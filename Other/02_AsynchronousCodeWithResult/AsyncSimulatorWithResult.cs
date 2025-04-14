using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;

namespace _02_AsynchronousCodeWithResult;

public static class AsyncSimulatorWithResult
{
  public static Task<int> ComputeValueAsync()
  {
    var stateMachine = new ComputeValueAsyncStateMachine();
    stateMachine.builder = AsyncTaskMethodBuilder<int>.Create();
  }

  public static Task<int> ReturnValueAsync()
  {
    return Task.Run(async () => {
      await Task.Delay(500);
      return 10;
    });
  }
}
