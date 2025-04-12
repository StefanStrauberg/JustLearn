if (args.Length == 0)
{
  Console.WriteLine("There aren't input arguments!");
  Console.WriteLine("Please enter arguments like: dotnet run /arg1 -arg2 arg3");
}
{
  foreach (var item in args)
    Console.WriteLine($"Arg: {item}");
}