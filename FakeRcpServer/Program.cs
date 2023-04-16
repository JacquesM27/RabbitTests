// See https://aka.ms/new-console-template for more information
using FakeRpcServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Hello, World!");
var host = new HostBuilder()
    .ConfigureServices(services =>
    {
        services.AddHostedService(
            serviceProvider => new FakeSeverManager("is_even_api"));
    })
    .UseConsoleLifetime()
    .Build();

host.Run();