namespace _05_MyAsyncAsynchornousRealisation2;

// Собственная реализация TaskCompletionSource
public class MyTaskCompletionSource<T>
{
  private readonly MyTask<T> _task = new();

  public MyTask<T> Task 
    => _task;
  
  public void SetResult(T result) 
    => _task.SetResult(result);

  public void SetException(Exception exception)
    => _task.SetException(exception);
}
