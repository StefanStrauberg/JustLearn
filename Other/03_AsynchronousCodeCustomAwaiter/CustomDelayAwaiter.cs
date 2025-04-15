using System.Runtime.CompilerServices;

namespace _03_AsynchronousCodeCustomAwaiter;

public class CustomDelayAwaiter(int delayMilliseconds) 
  : INotifyCompletion
{
  private readonly int _delayMilliseconds = delayMilliseconds;
  private Action _continuation = default!;
  private Timer? _timer = default!;
  // In our case it's always false
  // to ensure asynchronous behavior
  public bool IsCompleted => false;

  // Register continuation that will be calling after delay
  public void OnCompleted(Action continuation)
  {
    _continuation = continuation;

    // Check that the timer isn't created again
    _timer?.Dispose();

    _timer = new Timer(new TimerCallback(TimerElapsed),
                       null,
                       _delayMilliseconds,
                       Timeout.Infinite);
  }

  void TimerElapsed(object? state)
  {
    _timer?.Dispose();
    _continuation?.Invoke();
  }

  public void GetResult() {}
}