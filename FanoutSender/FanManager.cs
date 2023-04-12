using RabbitMQ.Client;
using System.Text;

namespace FanoutSender
{
    internal class FanManager
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private string _exchangeName;

        public FanManager()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = 8180,
                UserName = "guest",
                Password = "guest",
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void CreateExchange(string exchange)
        {
            _exchangeName = exchange;
            _channel.ExchangeDeclare(
                exchange: _exchangeName,
                type: ExchangeType.Fanout,
                durable: false,
                autoDelete: false,
                arguments: null);
        }

        public void BindNewQueue(string queueName, string routingKey)
        {
            _channel.QueueDeclare(
                queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            _channel.QueueBind(
               queue: queueName,
               exchange: _exchangeName,
               routingKey: routingKey,
               arguments: null);
        }

        public void SendMessageOnFanoutExchange(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(
                exchange: _exchangeName,
                routingKey: string.Empty,
                basicProperties: null,
                body: body);
        }
    }
}
