using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;

internal class HttpGetAwaitable(string host,
                                string path = "/") 
  : INotifyCompletion
{
  private Action? _continuation;
  private string _result = string.Empty;
  private bool _isCompleted = false;

  public HttpGetAwaitable GetAwaiter() 
    => this;

  public bool IsCompleted  { get => _isCompleted; }

  public void OnCompleted(Action continuation)
  {
    _continuation = continuation;

    var socket = new Socket(AddressFamily.InterNetwork,
                            SocketType.Stream,
                            ProtocolType.Tcp);

    socket.BeginConnect(host, 80, ar => 
    {
      socket.EndConnect(ar);

      var request = $"GET {path} HTTP/1.1\r\nHOST: {host}\r\nConnection: Close\r\n\r\n";
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

      void ReceiveCallback(IAsyncResult iar)
      {
        int bytesRead = socket.EndReceive(iar);
        if (bytesRead > 0)
        {
          sb.Append(Encoding.UTF8.GetString(buffer,
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
          _isCompleted = true;
          _result = sb.ToString();
          _continuation?.Invoke();
        }
      }
    }, null);
  }

  public string GetResult()
    => _result;
}
