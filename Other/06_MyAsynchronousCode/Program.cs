using _06_MyAsynchronousCode;

var consoleWriter = new ConsoleWriter('*');
var writer = consoleWriter.StartWriteAsync();

while (!writer.IsCompleted)
{
  Console.Write('.');
  Task.Delay(250).Wait();
}

await writer;

Console.WriteLine("Async operation completed!");