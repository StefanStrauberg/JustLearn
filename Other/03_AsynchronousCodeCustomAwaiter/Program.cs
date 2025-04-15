using _03_AsynchronousCodeCustomAwaiter;

Console.WriteLine(new string('*', 25));
Console.WriteLine("Custom async state mchine");
Console.WriteLine("In this case we implement with custom awaiter");
Console.WriteLine(new string('*', 25));

await TestCustomDelayAsync();
Console.WriteLine("Main method was complited");

static async Task TestCustomDelayAsync()
{
  Console.WriteLine("Start of delay with using custom awaiter.");
  await new CustomDelay(2000);
  Console.WriteLine("Delay was complited");
}