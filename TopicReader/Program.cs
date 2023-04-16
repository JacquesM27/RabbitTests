// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Welcome to the Topic Zoo!");

var factory = new ConnectionFactory
{
    HostName = "localhost",
    Port = 8180,
    UserName = "guest",
    Password = "guest",
};

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
var queueName = channel.QueueDeclare().QueueName;
Console.WriteLine("Are you zoo director? Type Yes or No");
string role = Console.ReadLine();
if (role.Equals("Yes"))
{
    queueName = "zoo_manager";
    Console.WriteLine("Nice to see you again sir");
    channel.QueueBind(queueName, "zoomanager", "");
}
else if (role.Equals("No"))
{
    Console.WriteLine("Type your routing key");
    string routingKey = Console.ReadLine();
    channel.QueueBind(queueName, "zoo", routingKey);
    //channel.QueueBind(queueName, "zoo", "zoo.#");
    //channel.QueueBind(queueName, "zoo", "zoo.penguin.*");
    //channel.QueueBind(queueName, "zoo", "zoo.*.barry");
}
Console.WriteLine("Waiting for messages");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var content = Encoding.UTF8.GetString(ea.Body.Span);
    Console.WriteLine($"New message!\t{content}\t{ea.RoutingKey}");
};
channel.BasicConsume(queueName, true, consumer);
Console.WriteLine("Press Enter to exit");
Console.ReadLine();