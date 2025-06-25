using GoogleTimezoneSln.Services;
using System.Text;
using GoogleTimezoneSln.Core;
using Microsoft.Extensions.Configuration;


internal class Program
{
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        // 1. Build configuration
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        IConfiguration config = builder.Build();

        // 2. Retrieve the API key from configuration
        // The path "GoogleTimezoneSettings:ApiKey" corresponds to the structure in appsettings.json
        var apiKey = config["GoogleTimezoneSettings:ApiKey"];

        if (string.IsNullOrEmpty(apiKey))
        {
            Console.WriteLine("API Key not found in appsettings.json. Please ensure it's configured correctly.");
            return;
        }

        var client = HttpClientFactorySingleton.Instance;
        var service = new TimeZoneService(client, apiKey);
        var app = new TimeZoneApp(service);

        await app.RunAsync();
    }
}