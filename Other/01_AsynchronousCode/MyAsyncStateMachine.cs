using System.Runtime.CompilerServices;

namespace _01_AsynchronousCode;

internal struct MyAsyncStateMachine() 
  : IAsyncStateMachine
{
  // field of state: -1 default balue
  // 0 after first await 
  // 1 - after second await
  public int state;
  // Task builder: manage creating and finishing tasks
  public AsyncTaskMethodBuilder builder;

  // Awaiter for a delay operation
  TaskAwaiter awaiter;

  public void MoveNext()
  {
    try
    {
      if (state == -1)
      {
        // Start processing the async method
        Console.WriteLine("Star working of an async method");
        // get awaiter from Task.Delay
        awaiter = Task.Delay(1000).GetAwaiter();

        // Check up is complited delay
        if (!awaiter.IsCompleted)
        {
          state = 0;
          builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
          return;
        }
      }

      // If state == 0, it means that that we returned after await.
      if (state == 0)
      {
        // Ending the wait. If the task failed, an exception will be thrown here.
        awaiter.GetResult();
        Console.WriteLine("Continue awaiting.");
      }

      // Method is completed successfully
      builder.SetResult();
    }
    catch (Exception ex)
    {
      // If and exception occurs, we pass it to the stack
      builder.SetException(ex);
    }
  }

  // Method to initialize state machine
  public void SetStateMachine(IAsyncStateMachine stateMachine)
  {
    builder.SetStateMachine(stateMachine);
  }
}