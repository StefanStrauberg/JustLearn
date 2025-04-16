using System.Runtime.CompilerServices;

namespace _05_MyAsyncAsynchornousRealisation2;

// Собственная реализация AsyncTaskMethodBuilder для асинхронных методов, 
// возвращающих MyTask<string>.
// Использует нашу реализацию MyTaskCompletionSource.
public struct MyAsyncTaskMethodBuilder
{
  // Внутренне используем MyTaskCompletionSource для создания "задачи"
  private MyTaskCompletionSource<string> _tcs;

  // Метод для создания билдера
  public static MyAsyncTaskMethodBuilder Create()
  {
    var Builder = new MyAsyncTaskMethodBuilder
    {
      _tcs = new MyTaskCompletionSource<string>()
    };
    return Builder;
  }

  // Свойство возвращает нашу задачу, 
  // которая завершится при вызове SetResult или SetException
  public readonly MyTask<string> Task 
    => _tcs.Task;

  // Завершает асинхронный метод успешно, устанавливая результат
  public readonly void SetResult(string result)
    => _tcs.SetResult(result);

  // Завершает асинхронный метод с ошибкой
  public readonly void SetException(Exception exception)
    => _tcs.SetException(exception);

  // Запускает state machine (вызывает MoveNext() на переданном state machine)
  public static void Start<TStateMachine>(ref TStateMachine stateMachine)
    where TStateMachine : IAsyncStateMachine
    => stateMachine.MoveNext();

  // Метод SetStateMachine для регистрации state machine
  // В упрощенной реализации он ничего не делает.
  public static void SetStateMachine(IAsyncStateMachine stateMachine)
  { }

  // Регистрирует продолжение через awaiter (для INotifyCompletion)
  public static void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, 
                                                               ref TStateMachine stateMachine)
    where TAwaiter : INotifyCompletion
    where TStateMachine : IAsyncStateMachine
    => awaiter.OnCompleted(stateMachine.MoveNext);

  // Регистрирует продолжение для ICriticalNotifyCompletion
  public static void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, 
                                                                     ref TStateMachine stateMachine)
    where TAwaiter : ICriticalNotifyCompletion
    where TStateMachine : IAsyncStateMachine
    => awaiter.UnsafeOnCompleted(stateMachine.MoveNext);
}
