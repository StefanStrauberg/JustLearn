using System.Runtime.CompilerServices;

namespace _06_MyAsynchronousCode;

public struct StateMachine : IAsyncStateMachine
{
  private ConsoleAwaitable _awaiter;

  public int State { get; set; }
  public AsyncTaskMethodBuilder Builder { get; init; }
  public int Delay { get; init; } // Задержка между символами (мс)
  public char Character { get; init; } // Символ для печати
  public int Count { get; init; } // Общее количество символов для печати

  public void MoveNext()
  {
    try
    {
      if (State == -1)
      {
        // Создаем awaiter, который начнет асинхронную печать
        _awaiter = new ConsoleAwaitable(Delay, Character, Count);

        if (!_awaiter.IsCompleted)
        {
          State = 0;
          Builder.AwaitUnsafeOnCompleted(ref _awaiter, ref this);
          return;
        }
      }

      if (State == 0)
      {
        // Получаем результат (в нашем случае просто завершение, 
        // GetResult ничего не возвращает)
        _awaiter.GetResult();
      }
      
      // Завершаем state machine, завершая задачу
      Builder.SetResult();
    }
    catch (Exception ex)
    {
      Builder.SetException(ex);
    }
  }

  public void SetStateMachine(IAsyncStateMachine stateMachine)
  {
    Builder.SetStateMachine(stateMachine);
  }
}
