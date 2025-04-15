using System.Runtime.CompilerServices;

namespace _05_MyAsyncAsynchornousRealisation2;

public struct CustomHttpStateMachine : IAsyncStateMachine
{
  public int State;
  public AsyncTaskMethodBuilder<string> Builder;
  public string Host;
  public string Path;
  HttpGetAwaitable _awaiter;

  public void MoveNext()
  {
    try
    {
      if (State == -1)
      {
        _awaiter = new HttpGetAwaitable(Host, Path);
        
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
