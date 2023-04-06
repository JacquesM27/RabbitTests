using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectSender
{
    public class BunnyExchangeService
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _exchange;

        public BunnyExchangeService(string exchangeName)
        {
            _connectionFactory = new()
            {
                HostName = "localhost",
                Port = 8180,
                UserName = "guest",
                Password = "guest",
            };
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _exchange = exchangeName;
            DeclareExchange();
        }
        
        private void DeclareExchange()
        {
            _channel.ExchangeDeclare(
                exchange: _exchange,
                type: "direct",
                durable: false,
                autoDelete: false,
                arguments: null);
        }


        public void DeclareQueue(string queue)
        {
            _channel.QueueDeclare(
                queue: queue,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        public void BindQueueToExchange(string queue, string routingKey)
        {
            _channel.QueueBind(
                queue: queue,
                exchange: _exchange,
                routingKey: routingKey,
                arguments: null);
        }

        public void PublishMessage(string routingKey, string message)
        {
            //var properties = _channel.CreateBasicProperties();
            //properties.Persistent = true;
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(
                exchange: _exchange,
                routingKey: routingKey,
                basicProperties: null,
                body: body);
        }
    }
}
