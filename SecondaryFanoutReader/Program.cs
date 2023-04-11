// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SecondaryFanoutReader;

Console.WriteLine("Hello, {cityName} ;)!\nType 1 for Gdansk\tType 2 for Poznan\tType nothing to exit");
var key = Console.ReadLine();
string city = string.Empty;
if (string.IsNullOrEmpty(key))
{
    Console.WriteLine("Bye bye...");
    return;
}
if (key == "1")
{
    city = "Gdansk";
}
else if (key == "2")
{
    city = "Poznan";
}
else
{
    Console.WriteLine("Learn to read!");
    return;
}

var host = new HostBuilder()
    .ConfigureServices(services =>
    {
        services.AddHostedService(
            serviceProvider =>
            new BackgroundBunny(city));
    })
    .UseConsoleLifetime()
    .Build();

host.Run();
