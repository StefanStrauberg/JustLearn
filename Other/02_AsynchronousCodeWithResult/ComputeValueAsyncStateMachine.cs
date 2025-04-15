using System.Runtime.CompilerServices;

namespace _02_AsynchronousCodeWithResult;

internal struct ComputeValueAsyncStateMachine
  : IAsyncStateMachine
{
  // field of state: -1 default value
  // 0 after first await 
  // 1 - after second await
  public int state;
  // Task builder
  public AsyncTaskMethodBuilder<int> builder;
  // Awaiter for the first delay (Task.Delay)
  TaskAwaiter awaiter1;
  // Awaiter for the second delay (ReturnValueAsync)
  TaskAwaiter<int> awaiter2;

  public void MoveNext()
  {
    int localResult = default;
    try
    {
      if (state == -1)
      {
        // Calling Task.Delay and getting awaiter
        awaiter1 = Task.Delay(1000)
                       .GetAwaiter();

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

        awaiter2 = Task.FromResult(123)
                       .GetAwaiter();

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
      }

      // Finish async method, set result
      builder.SetResult(localResult);
    }
    catch (Exception ex)
    {
      // In a case of an exception we pass it to the builder
      builder.SetException(ex);
    }
  }
  // This method is required by the IAsyncStateMachine
  public void SetStateMachine(IAsyncStateMachine stateMachine)
  {
    builder.SetStateMachine(stateMachine);
  }
}
