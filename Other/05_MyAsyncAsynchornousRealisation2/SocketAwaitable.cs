using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace _05_MyAsyncAsynchornousRealisation2;

public class SocketAwaitable : INotifyCompletion
{
  private Action? _continuation;
  private Socket _socket;
  private byte[] _buffer;
  private int _byteReceived;

  public SocketAwaitable(Socket socket, byte[] buffer)
  {
    _socket = socket;
    _buffer = buffer;
  }

  public SocketAwaitable GetAwaiter() 
    => this;

  public bool IsCompleted 
    => false;

  public void OnCompleted(Action continuation)
  {
    _continuation = continuation;

    _socket.BeginReceive(_buffer, 
                         0, 
                         _buffer.Length, 
                         SocketFlags.None, 
                         asyncResult =>
    {
      try
      {
        _byteReceived = _socket.EndReceive(asyncResult);
        _continuation?.Invoke();
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Receive error: {ex.Message}");
      }
    }, null);
  }

  public int GetResult() 
    => _byteReceived;
}
