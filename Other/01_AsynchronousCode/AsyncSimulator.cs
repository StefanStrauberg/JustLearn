using System.Runtime.CompilerServices;

namespace _01_AsynchronousCode;

public static class AsyncSimulator
{
  // Calling hte async method through the generated state machine
  public static Task RunAsync()
  {
    // Create instance of state machine
    var stateMachine = new MyAsyncStateMachine
    {
      // Initialize task builder 
      builder = AsyncTaskMethodBuilder.Create(),
      // Initial state
      state = -1
    };
    // Run machine state processing
    stateMachine.builder.Start(ref stateMachine);
    // Return Task that will be complited when the method is complited
    return stateMachine.builder.Task;
  }
}
