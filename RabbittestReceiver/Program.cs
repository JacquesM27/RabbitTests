// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbittestReceiver;

Console.WriteLine("Hello, World!");


var host = new HostBuilder()
    //.ConfigureHostConfiguration(hostBuilder =>
    //{ })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<WorkerHostedService>();
    })
    .UseConsoleLifetime()
    .Build();

host.Run();