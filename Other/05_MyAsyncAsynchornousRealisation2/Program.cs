using _05_MyAsyncAsynchornousRealisation2;

Console.WriteLine(new string('*', 25));
Console.WriteLine("Custom async state mchine");
Console.WriteLine("In this case we compound custom awaiter and state machine");
Console.WriteLine(new string('*', 25));

var request = CustomHttpClient.GetPageAsync("http://2ip.ru");
var response = await request;
Console.WriteLine(response);