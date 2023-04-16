// See https://aka.ms/new-console-template for more information
using TopicSender;

Console.WriteLine("Welcome to the zoo! You can send a message to an animal here!");

string exchangeName = "zoo";
string exchangeAlternateName = "zoomanager";
TopicManager _manager = new();
_manager.CreateExchangeAlternate(exchangeAlternateName);
_manager.BindNewQueue(exchangeAlternateName, "zoo_manager", "");
_manager.CreateExchange(exchangeName, exchangeAlternateName);
_manager.BindNewQueue(exchangeName, "thomas_penguin", "zoo.penguin.thomas");
_manager.BindNewQueue(exchangeName, "barry_penguin", "zoo.penguin.barry");
_manager.BindNewQueue(exchangeName, "barry_duck", "zoo.duck.barry");
_manager.BindNewQueue(exchangeName, "daisy_duck", "zoo.duck.daisy");
_manager.BindNewQueue(exchangeName, "donald_duck", "zoo.duck.donald");

while (true)
{
    Console.WriteLine("CTRL + C to exit");
    Console.WriteLine("Write routing key");
    string routingKey = Console.ReadLine();
    Console.WriteLine("Write message");
    string message = Console.ReadLine();
    string completeMessage = $"{Guid.NewGuid()} | {message}";
    _manager.PublishMessage(exchangeName, routingKey, completeMessage);
    Console.WriteLine($"Message sent! routing: {routingKey} | Message: {completeMessage}");
}
