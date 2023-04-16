using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace FakeRpcServer
{
    internal class FakeSeverManager : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queueName;

        public FakeSeverManager(string queueName)
        {
            _queueName = queueName;
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = 8180,
                UserName = "guest",
                Password = "guest",
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            DeclareQueue();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            _channel.BasicConsume(
                queue: _queueName,
                autoAck: false,
                consumer: consumer);

            consumer.Received += HandleReceivedMessage;
            //consumer.Received += (model, ea) =>
            //{
            //    HandleReceivedMessage(model, ea);
            //};
            return Task.CompletedTask;
        }

        private void HandleReceivedMessage(object? sender, BasicDeliverEventArgs eventArgs)
        {
            string response = string.Empty;
            var body = eventArgs.Body.ToArray();
            var props = eventArgs.BasicProperties;
            var replyProps = _channel.CreateBasicProperties();
            replyProps.CorrelationId = props.CorrelationId;

            try
            {
                var message = Encoding.UTF8.GetString(body);
                int msgNumber = int.Parse(message);
                Console.WriteLine($"IsEven({message})");
                response = IsEven(msgNumber).ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FakeServer error! {ex.Message}");
                response = string.Empty;
            }
            finally
            {
                var responseBytes = Encoding.UTF8.GetBytes(response);
                _channel.BasicPublish(
                    exchange: string.Empty,
                    routingKey: props.ReplyTo,
                    basicProperties: replyProps,
                    body: responseBytes);
                _channel.BasicAck(
                    deliveryTag: eventArgs.DeliveryTag,
                    multiple: false);
            }
        }

        private bool IsEven(int someNumber)
        {
            return someNumber % 2 == 0;
        }

        private void DeclareQueue()
        {
            _channel.QueueDeclare(
                queue: _queueName,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            _channel.BasicQos(
                prefetchSize: 0,
                prefetchCount: 1,
                global: false);
        }
    }
}