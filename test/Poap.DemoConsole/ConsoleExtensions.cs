using Newtonsoft.Json.Linq;

namespace Poap.DemoConsole;

public static class ConsoleEx
{
    public static void WriteJson(string message)
    {
        JObject parsed = JObject.Parse(message);

        foreach (var pair in parsed)
        {
            Console.WriteLine("{0}: {1}", pair.Key, pair.Value);
        }
    }
}
