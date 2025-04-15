using System.Runtime.CompilerServices;

namespace _05_MyAsyncAsynchornousRealisation2;

// Реализация state machine для асинхронного метода получения страницы
// Эта структура реализует IAsyncStateMachine, что позволяет компилятору 
// генерировать await-метод.
public struct CustomHttpStateMachine : IAsyncStateMachine
{
  // Поле, отображающее текущее состояние 
  // (например, -1 до первого await, 0 после первого await)
  public int State;
  // Билдер, управляющий созданием и завершением задачи
  public MyAsyncTaskMethodBuilder Builder;
  public string Url;
  // Awaiter, который будет получать данные по HTTP
  HttpGetAwaitable _awaiter;

  // Метод MoveNext вызывается для перехода 
  // от одного состояния state machine к следующему.
  public void MoveNext()
  {
    try
    {
      // Если state = -1, значит мы только начинаем 
      // выполнение асинхронного метода
      if (State == -1)
      {
        // Создаём awaiter, который инициирует HTTP-запрос
        _awaiter = new HttpGetAwaitable(Url);
        
         // Если awaiter ещё не завершён 
         // (асинхронная операция не закончилась)
        if (!_awaiter.IsCompleted)
        {
          // Переходим в состояние 0 (ждём завершения awaiter-а)
          State = 0;
          // Регистрируем continuation через AwaitOnCompleted
          // это означает, что когда awaiter завершится, 
          // будет вызвано MoveNext() вновь.
          MyAsyncTaskMethodBuilder.AwaitOnCompleted(ref _awaiter, ref this);
          // Выходим, оставляя метод для продолжения позже
          return;
        }
      }
      // Если state == 0, значит, выполнение возобновлено 
      // после окончания awaiter-а
      if (State == 0)
      {
        // Получаем результат асинхронной операции через awaiter
        var result = _awaiter.GetResult();
        // Завершаем задачу с результатом, вызывая SetResult
        Builder.SetResult(result);
      }
    }
    // Если происходит исключение, передаем его в Builder, 
    // чтобы завершить задачу ошибкой.
    catch (Exception ex)
    {
      Builder.SetException(ex);
    }
  }
  
  // Метод SetStateMachine используется для регистрации state machine 
  // (обычно не требует реализации в упрощённых вариантах)
  public readonly void SetStateMachine(IAsyncStateMachine stateMachine)
    => MyAsyncTaskMethodBuilder.SetStateMachine(stateMachine);
}
