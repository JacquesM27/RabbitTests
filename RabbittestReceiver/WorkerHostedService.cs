using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbittestReceiver
{
    public class WorkerHostedService : BackgroundService
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        public WorkerHostedService()
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
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.Span);
                HandleMessage(content, ea.RoutingKey);
                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume("test1-queue", false, consumer);

            //while(!stoppingToken.IsCancellationRequested) { }
            return Task.CompletedTask;
        }

        public void HandleMessage(string content, string routingKey)
        {
            Console.WriteLine($"content: {content} routingKey: {routingKey}");
        }
    }
}
