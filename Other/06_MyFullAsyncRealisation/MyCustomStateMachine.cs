using System.Runtime.CompilerServices;

namespace _06_MyFullAsyncRealisation;

internal struct MyCustomStateMachine : IAsyncStateMachine
{
  public int State { get; set; }
  public AsyncTaskMethodBuilder<string> Builder { get; set; }
  public string Host { get; set; }
  public string Path { get; set; }
  private MyCustomHttpAwaiter _awaiter;

  public void MoveNext()
  {
    try
    {
      if (State == -1)
      {
        _awaiter = new MyCustomHttpAwaiter(Host, Path);

        if (!_awaiter.IsCompleted)
        {
          State = 0;
          Builder.AwaitOnCompleted(ref _awaiter, ref this);
          return;
        }
      }

      if (State == 0)
      {
        var result = _awaiter.GetResult();
        Builder.SetResult(result);
      }
      
    }
    catch (Exception ex)
    {
      Builder.SetException(ex);
    }
  }

  public void SetStateMachine(IAsyncStateMachine stateMachine)
    => Builder.SetStateMachine(stateMachine);
}
