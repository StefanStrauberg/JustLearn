using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;


// Реализация awaiter-а, который получает HTTP-ответ через сокеты.
// Реализует INotifyCompletion, что позволяет await-оператору работать с ним.
internal class HttpGetAwaitable : INotifyCompletion
{
  private readonly string _url;
  private readonly string _host;
  private readonly string _path;
  // Здесь будет храниться полученный HTML
  private string _result = string.Empty;
  // Флаг завершения асинхронной операции
  private bool _isCompleted = false;
  // Сохранённый continuation, который вызовется 
  // при завершении асинхронной операции
  private Action? _continuation;
  private bool _wasInvoked = false;

  public HttpGetAwaitable(string url)
  {
    _url = url;
    
    // Парсим URL вручную (https://host/path → host, /path)
    if (_url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
    {
      var noHttps = _url["https://".Length..];
      var slashIndex = noHttps.IndexOf('/');
      if (slashIndex > -1)
      {
        _host = noHttps[..slashIndex];
        _path = noHttps[slashIndex..];
      }
      else
      {
        _host = noHttps;
        _path = "/";
      }
    }
    else if (_url.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
    {
        var noHttp = _url["http://".Length..];
        int slashIndex = noHttp.IndexOf('/');
        if (slashIndex > -1)
        {
            _host = noHttp[..slashIndex];
            _path = noHttp[slashIndex..];
        }
        else
        {
            _host = noHttp;
            _path = "/";
        }
    }
    else
    {
        _host = _url;
        _path = "/";
    }
  }

  // Свойство IsCompleted сообщает, завершилась ли асинхронная операция.
  public bool IsCompleted => _isCompleted;

  // Метод GetAwaiter возвращает самого себя, 
  // что упрощает реализацию awaitable.
  public HttpGetAwaitable GetAwaiter()  => this;

  // Возвращает результат после завершения операции
  public string GetResult() => _result;

  // Метод вызывается компилятором, чтобы "подписаться" на завершение 
  // (в continuation будет MoveNext из state machine)
  public void OnCompleted(Action continuation)
  {
    _continuation = continuation;

    bool useSsl = _url.StartsWith("https://",
                                  StringComparison.OrdinalIgnoreCase);
    
    // Создаем TCP-сокет 
    var socket = new Socket(AddressFamily.InterNetwork,
                            SocketType.Stream,
                            ProtocolType.Tcp);
    int port = useSsl ? 443 : 80;

    // Начинаем подключение к хосту
    socket.BeginConnect(_host, port, ar => 
    {
      try
      {
        // Завершаем подключение
        socket.EndConnect(ar);

        if (useSsl)
        {
          // Оборачиваем сокет в безопасный SSL-поток
          var networkStream = new NetworkStream(socket, ownsSocket: true);
          var sslStream = new SslStream(networkStream,
                                        false,
                                        ValidateServerCertificate!);
          // TLS handshake
          sslStream.AuthenticateAsClient(_host);
          
          // Обрабатываем HTTPS-запрос через SSL
          ProcessRequest(sslStream);
        }
        else
        {
          // Обрабатываем HTTP-запрос через обычный сокет
          ProcessRequest(null);
        }
      }
      catch (Exception ex)
      {
        // Обработка ошибок подключения
        _result = $"Connection error: {ex.Message}";
        _isCompleted = true;
        _continuation?.Invoke();
      }
    }, null);

    // Унифицированный метод отправки запроса и обработки ответа 
    // (через сокет или sslStream)
    void ProcessRequest(Stream? secureStream)
    {
      // Формируем HTTP-запрос с нужными заголовками
      var requestBytes = Encoding.ASCII.GetBytes($"GET {_path} HTTP/1.1\r\n" +
                                                 $"Host: {_host}\r\n" +
                                                 "User-Agent: curl/7.64.1\r\n" +
                                                 "Connection: Close\r\n\r\n");
      if (secureStream is not null)
      {
        // HTTPS-запрос
        secureStream.Write(requestBytes,
                           0,
                           requestBytes.Length);
        var buffer = new byte[8192];
        var sb = new StringBuilder();

        secureStream.BeginRead(buffer,
                               0,
                               buffer.Length,
                               ReadCallBack,
                               secureStream);

        void ReadCallBack(IAsyncResult ar)
        {
          try
          {
            var stream = (SslStream)ar.AsyncState!;
            int bytesRead = stream.EndRead(ar);
            
            if (bytesRead > 0)
            {
                sb.Append(Encoding.UTF8
                                  .GetString(buffer,
                                             0,
                                             bytesRead));
                stream.BeginRead(buffer,
                                 0,
                                 buffer.Length,
                                 ReadCallBack,
                                 stream);
            }
            else
            {
                stream.Close();
                _result = sb.ToString();
                _isCompleted = true;
                if (!_wasInvoked)
                {
                    _wasInvoked = true;
                    // Возобновляем работу state machine
                    _continuation?.Invoke();
                }
            }
          }
          catch (Exception ex)
          {
            _result = $"Read error: {ex.Message}";
            _isCompleted = true;
            if (!_wasInvoked)
            {
              _wasInvoked = true;
              _continuation?.Invoke();
            }
          }
        }
      }
      else
      {
        // HTTP-запрос
        socket.Send(requestBytes);
        var buffer = new byte[8192];
        var sb = new StringBuilder();

        socket.BeginReceive(buffer,
                            0,
                            buffer.Length,
                            SocketFlags.None,
                            ReceiveCallback,
                            null);

        void ReceiveCallback(IAsyncResult ar)
        {
          try
          {
            int bytesRead = socket.EndReceive(ar);
            
            if (bytesRead > 0)
            {
                sb.Append(Encoding.UTF8
                                  .GetString(buffer,
                                             0,
                                             bytesRead));
                socket.BeginReceive(buffer,
                                    0,
                                    buffer.Length,
                                    SocketFlags.None,
                                    ReceiveCallback,
                                    null);
            }
            else
            {
              socket.Close();
              _result = sb.ToString();
              _isCompleted = true;
              if (!_wasInvoked)
              {
                  _wasInvoked = true;
                  // Возобновляем работу state machine
                  _continuation?.Invoke();
              }
            }
          }
          catch (Exception ex)
          {
            _result = $"Read error: {ex.Message}";
            _isCompleted = true;
            if (!_wasInvoked)
            {
              _wasInvoked = true;
              _continuation?.Invoke();
            }
          }
        }
      }
    }
  }

  // Простейший валидатор SSL-сертификата — всегда возвращает true (не проверяет!)
  private static bool ValidateServerCertificate(object sender,
                                                X509Certificate certificate,
                                                X509Chain chain,
                                                SslPolicyErrors sslPolicyErrors)
  {
    return true;
  }
}
