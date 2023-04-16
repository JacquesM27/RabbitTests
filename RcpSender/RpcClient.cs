using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpcSender
{
    internal class RpcClient : IDisposable
    {
        private readonly string _queueName = "is_even_api";
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _replyQueueName;
        private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> _callbackMapper = new();

        public RpcClient()
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
            _replyQueueName = _channel.QueueDeclare().QueueName;
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += HandleMessage;

            _channel.BasicConsume(
                consumer: consumer,
                queue: _replyQueueName,
                autoAck: true);
        }

        private void HandleMessage(object? sender, BasicDeliverEventArgs ea)
        {
            if(!_callbackMapper.TryRemove(ea.BasicProperties.CorrelationId, out TaskCompletionSource<string>? taskCompletionSource)) 
            {
                return;
            }

            var body = ea.Body.ToArray();
            var response = Encoding.UTF8.GetString(body);
            taskCompletionSource.TrySetResult(response);
        }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }

        public Task<string> CallAsync(string message, CancellationToken cancellationToken = default)
        {
            IBasicProperties basicProperties = _channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            basicProperties.CorrelationId = correlationId;
            basicProperties.ReplyTo = _replyQueueName;
            var messageBytes = Encoding.UTF8.GetBytes(message);
            var taskCompletionSource = new TaskCompletionSource<string>();
            _callbackMapper.TryAdd(correlationId, taskCompletionSource);

            _channel.BasicPublish(
                exchange: string.Empty,
                routingKey: _queueName,
                basicProperties: basicProperties,
                body: messageBytes);

            cancellationToken.Register(() => _callbackMapper.TryRemove(correlationId, out _));
            return taskCompletionSource.Task;
        }
    }
}
