using System.Runtime.CompilerServices;

namespace _03_AsynchronousCodeCustomAwaiter;

public class CustomDelayAwaiter(int delayMilliseconds) 
  : INotifyCompletion
{
  private readonly int _delayMilliseconds = delayMilliseconds;
  private Action _continuation = default!;
  private Timer _timer = default!;
  // In our case it's always false
  // to ensure asynchronous behavior
  public bool IsCompleted => false;

  // Register continuation that will be calling after delay
  public void OnCompleted(Action continuation)
  {
    _continuation = continuation;

    _timer = new Timer( state => 
    {
      _timer.Dispose();
      // Call continuation
      _continuation?.Invoke();
    }, null, _delayMilliseconds, Timeout.Infinite);
  }

  public void GetResult() {}
}