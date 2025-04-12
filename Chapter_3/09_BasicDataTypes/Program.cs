Console.Write(new string('*', 5));
Console.Write(" Basic Data Types ");
Console.WriteLine(new string('*', 5));

LocalVarDeclarationV1();
LocalVarDeclarationV2();
LocalVarDeclarationV3();
DefaultDeclaration();
NewingDataTypes();

static void LocalVarDeclarationV1()
{
  Console.WriteLine("=> Data Declaration:");
#pragma warning disable CS0168 // Variable is declared but never used
  int myInt;
#pragma warning restore CS0168 // Variable is declared but never used
#pragma warning disable CS0168 // Variable is declared but never used
  string myString;
#pragma warning restore CS0168 // Variable is declared but never used
  Console.WriteLine("Like: int myInt;");
  Console.WriteLine("Like: string myString;");
  Console.WriteLine(new string('*', 25));
}

static void LocalVarDeclarationV2()
{
  Console.WriteLine("=> Data Declaration with Initialization:");
#pragma warning disable CS0219 // Variable is assigned but its value is never used
  int myInt = 0;
#pragma warning restore CS0219 // Variable is assigned but its value is never used
#pragma warning disable CS0219 // Variable is assigned but its value is never used
  string myString = "Test";
#pragma warning restore CS0219 // Variable is assigned but its value is never used
  Console.WriteLine("Like: int myInt = 0;");
  Console.WriteLine("Like: string myString = \"Test\";");
  Console.WriteLine(new string('*', 25));
}

static void LocalVarDeclarationV3()
{
  Console.WriteLine("=> Data Declaration several items in 1 line:");
#pragma warning disable CS0219 // Variable is assigned but its value is never used
  bool b1 = true, b2 = false, b3 = b1;
#pragma warning restore CS0219 // Variable is assigned but its value is never used
  Console.WriteLine("Like: bool b1 = true, b2 = false, b3 = b1;");
  Console.WriteLine(new string('*', 25));
}

static void DefaultDeclaration()
{
  Console.WriteLine("=> Default Declaration:");
#pragma warning disable CS0219 // Variable is assigned but its value is never used
  int myInt = default;
#pragma warning restore CS0219 // Variable is assigned but its value is never used
  Console.WriteLine("Like: int myInt = default;");
  Console.WriteLine(new string('*', 25));
}

static void NewingDataTypes()
{
  Console.WriteLine("=> Using new to create variables:");
#pragma warning disable CS0219 // Variable is assigned but its value is never used
  bool myBool = new();
#pragma warning restore CS0219 // Variable is assigned but its value is never used
#pragma warning disable CS0219 // Variable is assigned but its value is never used
  int myInt = new();
#pragma warning restore CS0219 // Variable is assigned but its value is never used
#pragma warning disable CS0219 // Variable is assigned but its value is never used
  DateTime myDate = new();
#pragma warning restore CS0219 // Variable is assigned but its value is never used
  Console.WriteLine("Like: bool myBool = new();");
  Console.WriteLine("Like: int myInt = new();");
  Console.WriteLine("Like: DateTime myDate = new();");
}