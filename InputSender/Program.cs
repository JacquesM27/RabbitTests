// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using System.Text;

Console.WriteLine("Hello, World!");

Console.Title = "Test2 sender";
Console.WriteLine("Test2 sender. Queue: test.2.queue");

var factory = new ConnectionFactory()
{
    HostName = "localhost",
    Port = 8180,
    UserName = "guest",
    Password = "guest"
};

using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.QueueDeclare(
        queue: "test.2.queue",
        durable: false,
        exclusive: false,
        autoDelete: false,
        arguments: null);

    while (true)
    {
        Console.WriteLine("write sms content");
        Console.WriteLine("type exit to exit");
        string content = Console.ReadLine();
        if (content.Equals("exit"))
        {
            break;
        }
        var body = Encoding.UTF8.GetBytes(content);

        channel.BasicPublish(
            exchange: "",
            routingKey: "test.2.queue",
            basicProperties: null,
            body);

        Console.WriteLine("sms sent\n\n");
    }

}
