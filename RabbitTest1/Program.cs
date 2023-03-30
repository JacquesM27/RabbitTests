// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using System.Text;

Console.WriteLine("Hello, World!");

while(true)
{
    string result = Console.ReadLine();

    ConnectionFactory _connectionFactory = new()
    {
        HostName = "localhost",
        Port = 8180,
        UserName = "guest",
        Password = "guest",
    };

    using var connection = _connectionFactory.CreateConnection();
    using var channel = connection.CreateModel();
    var body = Encoding.UTF8.GetBytes($"{result} {DateTime.Now}");
    IBasicProperties properties = channel.CreateBasicProperties();
    properties.ContentType = "application/json";
    channel.BasicPublish(
        exchange: "test1",
        routingKey: "dupadupa",
        basicProperties: properties,
        body: body);
}