using System.Runtime.CompilerServices;

namespace _05_MyAsyncAsynchornousRealisation2;

public static class CustomHttpClient
{
  public static Task<string> GetPageAsync(string host, string path = "/")
  {
    var stateMachine = new CustomHttpStateMachine
    {
      Builder = AsyncTaskMethodBuilder<string>.Create(),
      State = -1,
      Host = host,
      Path = path
    };
    stateMachine.Builder.Start(ref stateMachine);
    return stateMachine.Builder.Task;
  }
}
