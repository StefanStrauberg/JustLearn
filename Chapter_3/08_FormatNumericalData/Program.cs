Console.Write(new string('*', 5));
Console.Write(" Format Numerical Data ");
Console.WriteLine(new string('*', 5));

FormatNumericalData();

static void FormatNumericalData()
{
  Console.WriteLine("The value 99999 in various formats:");
  Console.WriteLine($"c format: {99999:c}");
  Console.WriteLine($"d9 format: {99999:d9}");
  Console.WriteLine($"f3 format: {99999:f3}");
  Console.WriteLine($"n format: {99999:n}");

  Console.WriteLine($"E format: {99999:E}");
  Console.WriteLine($"e format: {99999:e}");
  Console.WriteLine($"X format: {99999:X}");
  Console.WriteLine($"x format: {99999:x}");
}