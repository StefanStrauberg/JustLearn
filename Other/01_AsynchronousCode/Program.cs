using _01_AsynchronousCode;

Console.WriteLine(new string('*', 25));
Console.WriteLine("Custom state machine");
Console.WriteLine("In this case we implement with Task result");
Console.WriteLine(new string('*', 25));

try
{
  await AsyncSimulator.RunAsync();
  Console.WriteLine("The async method was completed.");
}
catch (Exception ex)
{
  Console.WriteLine($"Error: {ex.Message}");
}