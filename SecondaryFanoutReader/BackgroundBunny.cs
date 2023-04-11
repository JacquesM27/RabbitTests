using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondaryFanoutReader
{
    internal class BackgroundBunny : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queue;

        public BackgroundBunny(string queue)
        {
            _queue = queue;
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

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.Span);
                Console.WriteLine($"New message!\t{content}\t{ea.RoutingKey}");
            };
            _channel.BasicConsume(_queue, true, consumer);
            return Task.CompletedTask;
        }
    }
}
