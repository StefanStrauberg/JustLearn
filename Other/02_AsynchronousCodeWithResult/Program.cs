using _02_AsynchronousCodeWithResult;

Console.WriteLine(new string('*', 25));
Console.WriteLine("Custom state machine");
Console.WriteLine("In this case we implement with Task<int> result");
Console.WriteLine(new string('*', 25));

try
{
  int result = await AsyncSimulatorWithResult.ComputeValueAsync();
  Console.WriteLine("The async method was completed.");
  Console.WriteLine($"Result: {result}");
}
catch (Exception ex)
{
  Console.WriteLine($"Error: {ex.Message}");
}