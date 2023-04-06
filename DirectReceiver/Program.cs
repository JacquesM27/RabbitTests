// See https://aka.ms/new-console-template for more information
using DirectReceiver;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Hello, World!\nType queue name:");

string queueName = Console.ReadLine();
if(queueName == null)
{
    Console.WriteLine("you have to type queue name");
    return;
}

var host = new HostBuilder()
    .ConfigureServices(services =>
    {
        services.AddHostedService(
            serviceProvider =>
            new WorkerHostedService(queueName));
    })
    .UseConsoleLifetime()
    .Build();

host.Run();