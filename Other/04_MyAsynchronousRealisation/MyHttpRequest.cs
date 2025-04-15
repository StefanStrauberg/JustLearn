namespace _04_MyAsynchronousRealisation;

public class MyHttpRequest(string url)
{
  private readonly string _url = url;
  public MyHttpRequestAwaiter GetAwaiter()
    => new(_url);
}
