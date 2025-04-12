var arguments = Environment.GetCommandLineArgs();
if (arguments.Length == 1)
{
  Console.WriteLine("There aren't input arguments!");
  Console.WriteLine("Please enter arguments like: dotnet run /arg1 -arg2 arg3");
  return;
}
for (int i = 1; i < arguments.Length; i++)
    Console.WriteLine($"Arg: {arguments[i]}");