using System.Runtime.CompilerServices;

namespace _05_MyAsyncAsynchornousRealisation2;

// Реализация state machine для асинхронной операции получения страницы
// Эта структура реализует IAsyncStateMachine, 
// позволяя нам управлять переходами между состояниями.
public struct CustomHttpStateMachine : IAsyncStateMachine
{
  public int State; // Текущее состояние (-1: до await, 0: после await)
  public MyAsyncTaskMethodBuilder Builder; // Билдер, управляющий созданием и завершением задачи
  public string Url; // Переданный URL, который содержит протокол, хост и путь
  HttpGetAwaitable _awaiter; // Наш awaitable, который выполняет HTTP-запрос

  // MoveNext вызывается для продолжения выполнения state machine
  public void MoveNext()
  {
    try
    {
      if (State == -1)
      {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"\t\tCurrent thread: {Environment.CurrentManagedThreadId}");
        Console.ResetColor();
        Console.WriteLine("\t\tState -1");
        Console.WriteLine("\t\tCreate an awaiter, thath will initiate an HTTP request");
        
        // Создаём awaiter, который инициирует HTTP-запрос
        _awaiter = new HttpGetAwaitable(Url);
        
        // Если асинхронная операция еще не завершена,
        // регистрируем continuation (MoveNext) через AwaitOnCompleted
        Console.WriteLine("\t\tIf the async operation isn't completed");
        if (!_awaiter.IsCompleted)
        {
          Console.ForegroundColor = ConsoleColor.Magenta;
          Console.WriteLine($"\t\tCurrent thread: {Environment.CurrentManagedThreadId}");
          Console.ResetColor();
          Console.WriteLine("\t\tChange state to 0");
          
          State = 0; // Переходим в состояние ожидания
          
          Console.WriteLine("\t\tRegister continuation (MoveNext) via AwaitOnCompleted");
          MyAsyncTaskMethodBuilder.AwaitOnCompleted(ref _awaiter, ref this);
          Console.WriteLine("\t\tExit to continue the execution later");
          
          return; // Выходим, чтобы продолжить выполнение позже
        }
      }

      if (State == 0)
      {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"\t\tCurrent thread: {Environment.CurrentManagedThreadId}");
        Console.ResetColor();
        Console.WriteLine("\t\tState 0");
        Console.WriteLine("\t\tWhen awaiter is completed, get result");
        
        // Когда awaiter завершился, получаем результат
        var result = _awaiter.GetResult();
        
        Console.WriteLine("\t\tComplete the task, set result");
        
        // Завершаем задачу, устанавливая результат
        Builder.SetResult(result);
      }
    }
    catch (Exception ex)
    {
      // Если возникает исключение, завершаем задачу с ошибкой
      Builder.SetException(ex);
    }
  }
  
  /// SetStateMachine используется для регистрации состояния state machine.
  // В нашем упрощенном случае реализация пустая.
  public readonly void SetStateMachine(IAsyncStateMachine stateMachine)
    => MyAsyncTaskMethodBuilder.SetStateMachine(stateMachine);
}
