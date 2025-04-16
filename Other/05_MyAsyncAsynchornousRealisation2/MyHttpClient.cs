namespace _05_MyAsyncAsynchornousRealisation2;

// Главный класс-клиент, с которого начинается асинхронная операция
internal class MyHttpClient
{
  // Метод GetAsync принимает URL и возвращает MyTask<string>
  // Результатом будет HTML, полученный с помощью нашего кастомного awaitable
  public MyTask<string> GetAsync(string url)
  {
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine($"\tCurrent thread: {Environment.CurrentManagedThreadId}");
    Console.ResetColor();
    Console.WriteLine("\tCreate state machine and initialize it");
    // Создаем state machine и инициализируем его параметры
    var stateMachine = new CustomHttpStateMachine
    {
      // Используем наш собственный билдер, который создает MyTaskCompletionSource<string>
      Builder = MyAsyncTaskMethodBuilder.Create(),
      State = -1, // Начальное состояние до первого await
      Url = url,
    };

    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine($"\tCurrent thread: {Environment.CurrentManagedThreadId}");
    Console.ResetColor();
    Console.WriteLine("\tStart state machine (it immediately calls MoveNext())");
    // Запускаем state machine (она сразу вызовет MoveNext())
    MyAsyncTaskMethodBuilder.Start(ref stateMachine);
    
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine($"\tCurrent thread: {Environment.CurrentManagedThreadId}");
    Console.ResetColor();
    Console.WriteLine("\tReturn MyTask<string>, that will finish when state machine calls SetResult/SetException");
    // Возвращаем задачу, которая завершится, когда state machine вызовет SetResult/SetException
    return stateMachine.Builder.Task;
  }
}
