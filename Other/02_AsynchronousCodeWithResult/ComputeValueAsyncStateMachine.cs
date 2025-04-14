using System.Runtime.CompilerServices;

namespace _02_AsynchronousCodeWithResult;

internal struct ComputeValueAsyncStateMachine : IAsyncStateMachine
{
  public int state = -1;
  int localResult = default;
  public AsyncTaskMethodBuilder<int> builder;
  TaskAwaiter awaiter1;
  TaskAwaiter<int> awaiter2;

  public void MoveNext()
  {
    int localResult = default;
    try
    {
      if (state == -1)
      {
        Console.WriteLine("ComputeValueAsync: Start, wait delay 1 sec");
        // Return Task.Delay and get awaiter
        Task delayTask = Task.Delay(1000);
        awaiter1 = delayTask.GetAwaiter();

        // If the delay hasn't been completed yet, regester the continuation and exit
        if (!awaiter1.IsCompleted)
        {
          state = 0;
          builder.AwaitUnsafeOnCompleted(ref awaiter1, ref this);
          return;
        }
      }
      // continue after first await
      if (state == 0)
      {
        awaiter1.GetResult();
        Console.WriteLine("ComputeValueAsync: Delay has been completed, calling ReturnValueAsync");
        
        // Calling async method ReturnValueAsync and get its waiting
        Task<int> valueTask = AsyncSimulatorWithResult.ReturnValueAsync();
        awaiter2 = valueTask.GetAwaiter();

        // If the task is not ready,
        // save the state and register the continuation
        if (!awaiter2.IsCompleted)
        {
          state = 1;
          builder.AwaitUnsafeOnCompleted(ref awaiter2, ref this);
          return;
        }
      }
      // continue after second await
      if (state == 1)
      {
        localResult = awaiter2.GetResult();
        Console.WriteLine($"ComputeValueAsync: received the {localResult} value from ReturnValueAsync.");
      }
      // Getting the result from ReturnValueAsync. 
      // If there is an error, it will be forward here
      localResult += 20;
      Console.WriteLine($"ComputeValueASync: the final result is {localResult}");
    }
    catch (Exception ex)
    {
      builder.SetException(ex);
    }
  }

  public void SetStateMachine(IAsyncStateMachine stateMachine)
  {
    builder.SetStateMachine(stateMachine);
  }
}
