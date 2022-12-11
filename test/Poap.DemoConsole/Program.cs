using Newtonsoft.Json;
using Poap;
using Poap.DemoConsole;

var options = new PoapClientOptions
{
    ApiKey = "",
    ClientId = "",
    ClientSecret = "",
    Audience = "",
};

var client = new PoapClient(options);

//await client.GetEventPoapsAsync("90811");

var token = await client.GetTokenAsync("6049148");

ConsoleEx.WriteJson(JsonConvert.SerializeObject(token));

await Task.Delay(2000);

var paginated = await client.GetEventsPaginatedAsync(offset: 10, limit: 20);

ConsoleEx.WriteJson(JsonConvert.SerializeObject(paginated));

Console.ReadLine();

