using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;

namespace _06_MyFullAsyncRealisation;

internal class MyCustomHttpAwaiter(string host,
                                   string path = "/")
  : INotifyCompletion
{
  private Action? _continuation;
  private string _result = string.Empty;
  private bool _isCompleted = false;
  public bool IsCompleted { get => _isCompleted; }

  public MyCustomHttpAwaiter GetAwaiter()
    => this;

  public string GetResult()
    => _result;
  
  public void OnCompleted(Action continuation)
  {
    _continuation = continuation;

    var socket = new Socket(AddressFamily.InterNetwork,
                            SocketType.Stream,
                            ProtocolType.Tcp);

    socket.BeginConnect(host, 443, ac => 
    {
      try
      {
        socket.EndConnect(ac);

        string request = $"GET {path} HTTP/1.1\r\n" +
                        $"HOST: {host}\r\n"+ 
                        "Connection: Close\r\n\r\n";
        byte[] requestBytes = Encoding.ASCII.GetBytes(request);
        
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
          var bytesRead = socket.EndReceive(ar);
          Console.WriteLine($"Received {bytesRead} bytes");
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
            Console.WriteLine("Server closed connection");
            _isCompleted = true;
            _result = sb.ToString();
            _continuation?.Invoke();
          } 
        }
      }
      catch (Exception ex)
      {
        _result = $"Connection error: {ex.Message}";
        _isCompleted = true;
        _continuation?.Invoke();
      }
    }, null);
  }
}