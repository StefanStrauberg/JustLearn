using _05_MyAsyncAsynchornousRealisation2;

Console.WriteLine(new string('*', 25));
Console.WriteLine("Custom async state mchine");
Console.WriteLine("In this case we compound custom awaiter and state machine");
Console.WriteLine(new string('*', 25));

Console.WriteLine(new string ('*', 25));
Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine($"Current thread: {Environment.CurrentManagedThreadId}");
Console.ResetColor();
Console.WriteLine(new string ('*', 25));

var client = new MyHttpClient();
var myTask = client.GetAsync("http://2ip.ru");

Console.WriteLine(new string ('*', 25));
Console.WriteLine("Main thread continues work");
Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine($"Current thread: {Environment.CurrentManagedThreadId}");
Console.ResetColor();
Console.WriteLine(new string ('*', 25));

while (!myTask.IsCompleted)
{
  Console.Write('.');
  Task.Run(() => Task.Delay(5)).Wait();
}

Console.WriteLine("Before await");

var response = await myTask;

Console.WriteLine("After await - the async operation is completed");

Console.WriteLine(new string ('*', 25));
Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine($"Current thread: {Environment.CurrentManagedThreadId}");
Console.ResetColor();
Console.WriteLine("Length of response: " + response.Length);
Console.WriteLine(new string ('*', 25));