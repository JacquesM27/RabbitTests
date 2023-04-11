// See https://aka.ms/new-console-template for more information
using DirectSender;

Console.WriteLine("Hellogh, Exchange Direct queue!");

string queue1 = "direct.test.1";
string queue2 = "direct.test.2";
string routingKey1 = "routing.key.1";
string routingKey2 = "routing.key.2";

BunnyExchangeService _exchange = new("myFirstExchange");
_exchange.DeclareQueue(queue1);
_exchange.BindQueueToExchange(queue1, routingKey1);
_exchange.DeclareQueue(queue2);
_exchange.BindQueueToExchange(queue2, routingKey2);

while (true)
{
    Console.WriteLine("Type 1 to send message on queue 1"+
        "\nType 2 to send message on quque 2"+
        "\nType 3 to send message on queues 1 and 2"+
        "\nType pastalavista to exit");
    string type = Console.ReadLine();
    var guid = Guid.NewGuid();
    switch (type)
    {
        case "1":
            _exchange.PublishMessage(routingKey1, $"On bike {guid}");
            break;
        case "2":
            _exchange.PublishMessage(routingKey2, $"By car {guid}");
            break;
        case "3":
            _exchange.PublishMessage(routingKey1, $"On bike {guid}");
            _exchange.PublishMessage(routingKey2, $"By car {guid}");
            break;
        case "pastalavista":
            return;
        default:
            break;
    }
}
