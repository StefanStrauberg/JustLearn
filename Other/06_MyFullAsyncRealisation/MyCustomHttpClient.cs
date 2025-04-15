using System.Runtime.CompilerServices;

namespace _06_MyFullAsyncRealisation;

internal struct MyCustomHttpClient
{
  public static Task<string> GetPageAsync(string host,
                                          string path = "/")
  {
    var stateMachine = new MyCustomStateMachine()
    {
      Host = host,
      Path = path,
      State = -1,
      Builder = AsyncTaskMethodBuilder<string>.Create()
    };
    stateMachine.Builder.Start(ref stateMachine);
    return stateMachine.Builder.Task;
  }
}
