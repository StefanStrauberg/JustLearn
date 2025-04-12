Console.Write(new string('*', 5));
Console.Write(" Basic Console IO ");
Console.WriteLine(new string('*', 5));

GetUserData(out string name, out int age);
SetColorConsole();
Print($"Hello {name}! You're {age} years old!");
Console.ResetColor();
Console.WriteLine();

static void SetColorConsole()
{
  Console.BackgroundColor = ConsoleColor.Yellow;
  Console.ForegroundColor = ConsoleColor.Blue;
}

static void Print(string message)
  => Console.Write(message);


static void GetUserData(out string name, out int age)
{
  Console.Write("Please enter your name: ");
  var input = Console.ReadLine();
  name = string.IsNullOrWhiteSpace(input) == true ? "unknown" : input;
  Console.Write("Please enter your age: ");
  if (Int32.TryParse(Console.ReadLine(), out age)) {}
  age = default;
}