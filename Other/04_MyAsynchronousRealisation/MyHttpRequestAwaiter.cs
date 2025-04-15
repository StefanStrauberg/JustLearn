using System.Net;
using System.Runtime.CompilerServices;

namespace _04_MyAsynchronousRealisation;

public class MyHttpRequestAwaiter(string url) : INotifyCompletion
{
  private string? _result;
  private Exception? _exception;
  private Action? _continuation;

  public bool IsCompleted => false;

  public void OnCompleted(Action continuation)
  {
    _continuation = continuation;
    var request = WebRequest.Create(url);
    request.Method = "GET";
    request.BeginGetResponse(OnResponse, request);
  }

  void OnResponse(IAsyncResult ar)
  {
    try
    {
      var request = (HttpWebRequest)ar.AsyncState!;
      using var response = (HttpWebResponse)request.EndGetResponse(ar);
      using var reader = new StreamReader(response.GetResponseStream());
      _result = reader.ReadToEnd();
    }
    catch (Exception ex)
    {
      _exception = ex;
    }
    _continuation?.Invoke();
  }

  public string GetResult()
  {
    if (_exception is not null)
      throw _exception;
    return _result!;
  }
}
