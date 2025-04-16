using System.Runtime.CompilerServices;

namespace _06_MyAsynchronousCode;

public class ConsoleAwaitable(int delay,
                              char character,
                              int count) : ICriticalNotifyCompletion
{
  private readonly int _delay = delay;
  private readonly char _character = character;
  private readonly int _count = count;

  private Action? _continuation; 
  private int _printed = 0; // количество уже напечатанных символов
  private Timer? _timer; // таймер для управления интервалом печати

  private bool _isCompleted = false;

  internal ConsoleAwaitable GetAwaiter() => this;

  public bool IsCompleted => _isCompleted;

  public void UnsafeOnCompleted(Action continuation)
  {
    _continuation = continuation;

    _timer = new Timer(_ => 
                       {
                         Console.Write(_character);
                         _printed++;
                         if (_printed >= _count)
                         {
                           _isCompleted = true;
                           _timer?.Dispose();
                           continuation?.Invoke();
                         }
                       },
                       null, 
                       0, 
                       _delay);
  }

  public void OnCompleted(Action continuation)
  {
    UnsafeOnCompleted(continuation);
  }

  internal void GetResult()
  { }
}
