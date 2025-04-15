namespace _05_MyAsyncAsynchornousRealisation2;

public class MyTask<T>
{
  private T _result = default!;
  private Exception? _exception;
  private bool _isCompleted = false;
  private readonly List<Action> _continuations = [];

  public bool IsCompleted 
    => _isCompleted;

  // Completes the task sucessufully
  // setting the result
  public void SetResult(T result)
  {
    if (_isCompleted)
      throw new InvalidOperationException("Task already completed.");

    _result = result;
    _isCompleted = true;

    foreach (var cont in _continuations)
      cont();
  }

  // Method returns a result or throws an exception 
  // if the tash failed
  public T GetResult() 
  {
    if (!_isCompleted)
      throw new InvalidOperationException("Task has not completed yet.");
    if (_exception != null)
      throw _exception;
    return _result;
  }

  public void OnCompleted(Action continuation)
  {
    if (_isCompleted)
      continuation();
    else
      _continuations.Add(continuation);
  }

  // Complete a task throwing an exception
  public void SetException(Exception exception)
  {
      if (_isCompleted)
          throw new InvalidOperationException("Task already completed.");

      _exception = exception;
      _isCompleted = true;
      
      foreach (var cont in _continuations)
          cont();
  }

  // Method to support the await pattern
  public MyTaskAwaiter<T> GetAwaiter() 
    => new(this);
}
