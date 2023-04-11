// See https://aka.ms/new-console-template for more information
using FanoutSender;
using System.Text;

Console.WriteLine("Hello, World!");

FanManager _fanManager= new();
_fanManager.CreateExchange("Warsaw");
_fanManager.BindNewQueue("Cracow", "warsaw.cracow");
_fanManager.BindNewQueue("Gdansk", string.Empty);
_fanManager.BindNewQueue("Poznan", "warsaw.poznan");

while (true)
{
    Console.WriteLine("Welcome to Warsaw radio!\nType message to other cities");
    Console.WriteLine("Type EXIT to close radio");
    string message = Console.ReadLine();
    if (message.Equals("EXIT"))
    {
        return;
    }
    DateTime dt = DateTime.Now;
    StringBuilder sb = new();
    sb.Append(dt.ToString("G"));
    sb.Append(" - ");
    sb.Append(message);
    _fanManager.SendMessageOnFanoutExchange(sb.ToString());
}
