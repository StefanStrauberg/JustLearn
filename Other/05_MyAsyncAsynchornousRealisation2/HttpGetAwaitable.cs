using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;


// Реализация awaitable для выполнения HTTP/HTTPS запроса с использованием сокетов
internal class HttpGetAwaitable : INotifyCompletion
{
  private readonly string _host; // Парсится из _url (например, "2ip.ru")
  private readonly string _path; // Парсится из _url (например, "/" или "/some/page")
  private string _result = string.Empty; // Здесь будет храниться полученный HTML
  private bool _isCompleted = false; // Флаг завершения асинхронной операции
  private Action? _continuation; // Сохраненный callback (continuation) для state machine
  private bool _wasInvoked = false; // Защита от повторного вызова continuation

  // Конструктор принимает URL, затем парсит его на host и path
  public HttpGetAwaitable(string url)
  {    
    if (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
    {
        var noHttp = url["http://".Length..];
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
        _host = url;
        _path = "/";
    }
  }

  // Свойство IsCompleted: возвращает флаг завершения асинхронной операции
  public bool IsCompleted => _isCompleted;

  // Метод GetAwaiter возвращает самого себя — упрощает использование awaitable
  public HttpGetAwaitable GetAwaiter()  => this;

  // Метод GetResult возвращает результат (HTML ответа)
  public string GetResult() => _result;

  // Метод OnCompleted вызывается компилятором, чтобы зарегистрировать continuation.
  // Здесь мы запускаем асинхронную операцию по получению HTTP-ответа.
  public void OnCompleted(Action continuation)
  {
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"\t\t\tCurrent thread: {Environment.CurrentManagedThreadId}");
    Console.ResetColor();
    Console.WriteLine("\t\t\tRegister the continuation");
    _continuation = continuation;
    
    Console.WriteLine("\t\t\tCreate a TCP-socket");
    // Создаем TCP-сокет для установления подключения
    var socket = new Socket(AddressFamily.InterNetwork,
                            SocketType.Stream,
                            ProtocolType.Tcp);

    // Начинаем асинхронное подключение к серверу
    socket.BeginConnect(_host, 80, ar => 
    {
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine($"\t\t\tCurrent thread: {Environment.CurrentManagedThreadId}");
      Console.ResetColor();
      Console.WriteLine("\t\t\tBegin connection to the server");
      try
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\t\t\tCurrent thread: {Environment.CurrentManagedThreadId}");
        Console.ResetColor();
        Console.WriteLine("\t\t\tEnd connection to the server");
        // Завершаем подключение
        socket.EndConnect(ar);

        // работаем напрямую через сокет
        ProcessRequest();
      }
      catch (Exception ex)
      {
        // В случае ошибки подключения сохраняем сообщение и завершаем операцию
        _result = $"Connection error: {ex.Message}";
        _isCompleted = true;
        _continuation?.Invoke();
      }
    }, null);

    // Локальный метод, который отправляет HTTP-запрос и обрабатывает ответ.
    // Если secureStream не null, значит используется HTTPS (SSL), иначе HTTP.
    void ProcessRequest()
    {
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine($"\t\t\tCurrent thread: {Environment.CurrentManagedThreadId}");
      Console.ResetColor();
      Console.WriteLine("\t\t\tCreate HTTP-request with predefined headers.");
      // Формируем HTTP-запрос с нужными заголовками.
      var requestBytes = Encoding.ASCII.GetBytes($"GET {_path} HTTP/1.1\r\n" +
                                                 $"Host: {_host}\r\n" +
                                                 "User-Agent: curl/7.64.1\r\n" +
                                                 "Connection: Close\r\n\r\n");
      
      Console.WriteLine("\t\t\tSend request via socket");
      // Oтправляем запрос через сокет напрямую
      socket.Send(requestBytes);
      var buffer = new byte[8192];
      var sb = new StringBuilder();

      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine($"\t\t\tCurrent thread: {Environment.CurrentManagedThreadId}");
      Console.ResetColor();
      Console.WriteLine("\t\t\tBegin receive data via socket");
      // Начинаем асинхронное чтение через сокет
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
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\t\t\tCurrent thread: {Environment.CurrentManagedThreadId}");
            Console.ResetColor();
            Console.WriteLine("\t\t\tRead and save received data");
            // Накапливаем данные, используя только полученное количество байт
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
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\t\t\tCurrent thread: {Environment.CurrentManagedThreadId}");
            Console.ResetColor();
            Console.WriteLine("\t\t\tThere is no more data, close the socket");
            // Если данные закончились, закрываем сокет, 
            // сохраняем результат и вызываем continuation
            socket.Close();
            _result = sb.ToString();
            _isCompleted = true;
            if (!_wasInvoked)
            {
                _wasInvoked = true;
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
