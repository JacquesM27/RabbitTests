// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Hello, Cracow!");

var factory = new ConnectionFactory
{
    HostName = "localhost",
    Port = 8180,
    UserName = "guest",
    Password = "guest",
};

using (var connection = factory.CreateConnection())
{
    using (var channel = connection.CreateModel())
    {
        channel.ExchangeDeclare("Warsaw",ExchangeType.Fanout);
        channel.QueueDeclare("Cracow", false, false, false, null);
        channel.QueueBind("Cracow", "Warsaw", "warsaw.cracow");

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var content = Encoding.UTF8.GetString(ea.Body.Span);
            Console.WriteLine($"New message!\t{content}\t{ea.RoutingKey}");
        };

        channel.BasicConsume("Cracow", true, consumer);

        Console.WriteLine("Type something to exit");
        Console.ReadLine();
    }
}