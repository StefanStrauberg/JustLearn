using System.Runtime.CompilerServices;

namespace _06_MyAsynchronousCode;

public class ConsoleWriter
{
  private readonly int _delay;
  private readonly char _charachter;
  private readonly int _count;

  public ConsoleWriter(char charachter)
  {
    var rnd = new Random();
    _delay = rnd.Next(100, 1500);
    _count = rnd.Next(5, 10);
    _charachter = charachter;
  }
  
  public ConsoleWriter() : this('+')
  { }

  public Task StartWriteAsync()
  {
    var builder = AsyncTaskMethodBuilder.Create();

    var stateMachine = new StateMachine()
    {
      Builder = builder,
      State = -1,
      Delay = _delay,
      Character = _charachter,
      Count = _count
    };

    builder.Start(ref stateMachine);

    return builder.Task;
  }  
}
