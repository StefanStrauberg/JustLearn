namespace _03_AsynchronousCodeCustomAwaiter;

public class CustomDelay(int delayMilliseconds)
{
  private readonly int _delayMilliseconds = delayMilliseconds;

  public CustomDelayAwaiter GetAwaiter()
  {
    return new CustomDelayAwaiter(_delayMilliseconds);
  }
}
