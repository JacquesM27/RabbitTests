using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopicSender
{
    internal class TopicManager
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public TopicManager()
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

        public void CreateExchangeAlternate(string alternateExchangeName)
        {
            _channel.ExchangeDeclare(
                exchange: alternateExchangeName,
                type: ExchangeType.Fanout,
                durable: false,
                autoDelete: false,
                arguments: null);
        }

        public void CreateExchange(string exchangeName, string alternateExchangeName)
        {
            Dictionary<string, object> arguments= new()
            {
                { "alternate-exchange", alternateExchangeName.ToString() }
            };
            
            _channel.ExchangeDeclare(
                exchange: exchangeName,
                type: ExchangeType.Topic,
                durable: false,
                autoDelete: false,
                arguments: arguments);
        }

        public void BindNewQueue(string exchangeName, string queueName, string routingKey)
        {
            DeclareQueue(queueName);
            BindQueue(exchangeName, queueName, routingKey);
        }

        public void PublishMessage(string exchangeName, string routingKey, string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(
                exchange: exchangeName,
                routingKey: routingKey,
                basicProperties: null,
                body: body);
        }

        private void DeclareQueue(string queueName)
        {
            _channel.QueueDeclare(
               queue: queueName,
               durable: false,
               exclusive: false,
               autoDelete: false,
               arguments: null);
        }

        private void BindQueue(string exchangeName, string queueName, string routingKey)
        {
            _channel.QueueBind(
               queue: queueName,
               exchange: exchangeName,
               routingKey: routingKey,
               arguments: null);
        }

    }
}
