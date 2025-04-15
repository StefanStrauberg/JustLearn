using System.Runtime.CompilerServices;

namespace _02_AsynchronousCodeWithResult;

public struct AsyncSimulatorWithResult
{
  public static Task<int> ComputeValueAsync()
  {
    var stateMachine = new ComputeValueAsyncStateMachine
    {
      builder = AsyncTaskMethodBuilder<int>.Create(),
      state = -1
    };
    stateMachine.builder.Start(ref stateMachine);
    return stateMachine.builder.Task;
  }

}
