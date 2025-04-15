using _06_MyFullAsyncRealisation;

Console.WriteLine(new string('*', 25));
Console.WriteLine("Custom async state mchine");
Console.WriteLine("In this case we compound custom awaiter and state machine");
Console.WriteLine(new string('*', 25));

var data = await MyCustomHttpClient.GetPageAsync("2ip.ru");
Console.WriteLine($"Received data: ");
Console.WriteLine(data);