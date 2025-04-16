namespace _05_MyAsyncAsynchornousRealisation2;

// Собственная реализация Task<T>
public class MyTask<T>
{
  private T _result = default!;
  private Exception? _exception;
  private bool _isCompleted = false;
  private readonly List<Action> _continuations = [];

  public bool IsCompleted => _isCompleted;

  // Завершаем задачу успешно и вызываем continuations
  public void SetResult(T result)
  {
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.WriteLine($"\t\t\t\tТCurrent thread: {Environment.CurrentManagedThreadId}");
    Console.ResetColor();
    Console.WriteLine("\t\t\t\tSet T result");
    
    if (_isCompleted)
      throw new InvalidOperationException("Task already completed.");

    _result = result;
    _isCompleted = true;

    foreach (var cont in _continuations)
      cont();
  }

  // Метод для получения результата (если задача завершена) 
  // или выбрасывания исключения
  public T GetResult() 
  {
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.WriteLine($"\t\t\t\tCurrent thread: {Environment.CurrentManagedThreadId}");
    Console.ResetColor();
    Console.WriteLine("\t\t\t\tGet T Result");
    
    if (!_isCompleted)
      throw new InvalidOperationException("Task has not completed yet.");
    if (_exception != null)
      throw _exception;
    return _result;
  }

  // Регистрация продолжения. Если задача уже завершена, 
  // continuation вызывается сразу
  public void OnCompleted(Action continuation)
  {
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.WriteLine($"\t\t\t\tCurrent thread: {Environment.CurrentManagedThreadId}");
    Console.ResetColor();
    Console.WriteLine("\t\t\t\tRegister the continuation");
    
    if (_isCompleted)
      continuation();
    else
      _continuations.Add(continuation);
  }

  // Завершаем задачу с ошибкой и вызываем continuations
  public void SetException(Exception exception)
  {
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.WriteLine($"\t\t\t\tCurrent thread: {Environment.CurrentManagedThreadId}");
    Console.ResetColor();
    Console.WriteLine("\t\t\t\tSet the exception");
    
    if (_isCompleted)
        throw new InvalidOperationException("Task already completed.");

    _exception = exception;
    _isCompleted = true;
    
    foreach (var cont in _continuations)
        cont();
  }

  // Поддержка await: возвращаем наш awaiter
  public MyTaskAwaiter<T> GetAwaiter() 
    => new(this);
}
