// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Hello, World!");


Console.Title = "Test2 consumer";
Console.WriteLine("Test2 consumer. Queue: test.2.queue");

var factory = new ConnectionFactory()
{
    HostName = "localhost",
    Port = 8180,
    UserName = "guest",
    Password = "guest"
};

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(
    queue: "test.2.queue",
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var content = Encoding.UTF8.GetString(ea.Body.Span);
    Console.WriteLine(content);
    channel.BasicAck(ea.DeliveryTag, false);
};

channel.BasicConsume(
    queue: "test.2.queue",
    autoAck: false,
    consumer: consumer);

while (true)
{
    Console.WriteLine("type exit to exit");
    string content = Console.ReadLine();
    if (content.Equals("exit"))
    {
        break;
    }
}