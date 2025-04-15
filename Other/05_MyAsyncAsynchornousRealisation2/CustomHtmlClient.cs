namespace _05_MyAsyncAsynchornousRealisation2;

// Главный класс-клиент, с которого начинается асинхронная операция
public static class CustomHttpClient
{
  // Статический метод, который начинает асинхронное получение веб-страницы.
  // Принимает host и (необязательно) path, возвращает Task<string> с полученным HTML.
  public static MyTask<string> GetPageAsync(string url)
  {
    // Создаём экземпляр state machine, передавая необходимые параметры
    var stateMachine = new CustomHttpStateMachine
    {
      // Инициализируем билдер асинхронного метода (создаётся через стандартный метод Create)
      Builder = MyAsyncTaskMethodBuilder.Create(),
      State = -1,
      Url = url,
    };
    // Запускаем state machine, вызовом Builder.Start(ref stateMachine)
    MyAsyncTaskMethodBuilder.Start(ref stateMachine);
    // Возвращаем задачу, которая завершится, когда state machine 
    // вызовет SetResult или SetException
    return stateMachine.Builder.Task;
  }
}
