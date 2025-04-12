Print("Some interesting Environment's features.");

Print(new string('*', 50));
Print($"MachineName: {Environment.MachineName}");
Print(($"UserName: {Environment.UserName}"));
Print($"OS: {Environment.OSVersion}");
Print($"System directory: {Environment.SystemDirectory}");
Print(new string('*', 50));

foreach (var item in Environment.GetLogicalDrives())
  Print($"Drive: {item}");

Print($"Number of logical CPU: {Environment.ProcessorCount}");
Print($"Process path: {Environment.ProcessPath}");
Print($"Current directory: {Environment.CurrentDirectory}");

static void Print(string message)
  => Console.WriteLine(message);