// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;

Console.WriteLine("Hello, World!");

Console.Title = "Test2 sender";
Console.WriteLine("Test2 sender. Queue: MyQueue.1");

var factory = new ConnectionFactory()
{
    HostName = "localhost",
    Port = 8180,
    UserName = "guest",
    Password = "guest"
}