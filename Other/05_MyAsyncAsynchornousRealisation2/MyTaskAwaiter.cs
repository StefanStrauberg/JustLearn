using System.Runtime.CompilerServices;

namespace _05_MyAsyncAsynchornousRealisation2;

// Awaiter для MyTask<T>
public class MyTaskAwaiter<T>(MyTask<T> task) 
  : INotifyCompletion
{
  private readonly MyTask<T> _task = task;

  public bool IsCompleted 
    => _task.IsCompleted;

  public T GetResult()
    => _task.GetResult();

  public void OnCompleted(Action continuation)
    => _task.OnCompleted(continuation);
}