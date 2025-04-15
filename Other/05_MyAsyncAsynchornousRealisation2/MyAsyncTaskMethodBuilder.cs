using System.Runtime.CompilerServices;

namespace _05_MyAsyncAsynchornousRealisation2;

public struct MyAsyncTaskMethodBuilder
{
  private MyTaskCompletionSource<string> _tcs;

  public static MyAsyncTaskMethodBuilder Create()
  {
    var Builder = new MyAsyncTaskMethodBuilder
    {
      _tcs = new MyTaskCompletionSource<string>()
    };
    return Builder;
  }

  // A property that return a Task<string> that will
  // terminate when SetResult or SetException is called
  public readonly MyTask<string> Task 
    => _tcs.Task;

  // The method that completes the asynchronous
  // method sucessfully with the result
  public readonly void SetResult(string result)
    => _tcs.SetResult(result);

  // The method that terminates the asynchronous
  // method with an error
  public readonly void SetException(Exception exception)
    => _tcs.SetException(exception);

  // Starts the state machine - MoveNext()
  // is called on the transfered state
  public static void Start<TStateMachine>(ref TStateMachine stateMachine)
    where TStateMachine : IAsyncStateMachine
    => stateMachine.MoveNext();

  // Allow set state machine
  public static void SetStateMachine(IAsyncStateMachine stateMachine)
  { }

  // Method for registring a continuation via an awaiter
  public static void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, 
                                                               ref TStateMachine stateMachine)
    where TAwaiter : INotifyCompletion
    where TStateMachine : IAsyncStateMachine
    => awaiter.OnCompleted(stateMachine.MoveNext);

  // Method for registring a continuation via an awaiter 
  // for ICriticalNotifyCompletion
  public static void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, 
                                                                     ref TStateMachine stateMachine)
    where TAwaiter : ICriticalNotifyCompletion
    where TStateMachine : IAsyncStateMachine
    => awaiter.UnsafeOnCompleted(stateMachine.MoveNext);
}
