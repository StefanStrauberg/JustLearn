using _04_MyAsynchronousRealisation;

Console.WriteLine(new string('*', 25));
Console.WriteLine("Custom async state mchine");
Console.WriteLine("In this case we http request to the website");
Console.WriteLine(new string('*', 25));

Console.WriteLine("🚀 Starting request...");
string html = await new MyHttpRequest("http://iddqd.ru");

Console.WriteLine($"✅ Request completed.");
Console.WriteLine($"✅ Request completed. HTML length: {html.Length}");