using api.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace api.Services
{
    public class ConsumerService : IHostedService, IDisposable
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly string _queueName;
        private readonly ILogger<ConsumerService> _logger;
        private readonly ITransactionService _transactionService;

        public ConsumerService(ILogger<ConsumerService> logger, ITransactionService transactionService)
        {
            _connectionFactory = new ConnectionFactory() { HostName = "localhost" };
            _queueName = "transaction";
            _logger = logger;
            _transactionService = transactionService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(_queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    Console.WriteLine("Mensage received: {0}", message);

                    var transaction = JsonSerializer.Deserialize<TransactionRequest>(message);

                    if (transaction is not null)
                    {
                        var result = _transactionService.Transact(transaction);

                        _logger.LogInformation(result.StatusMessage);
                    }

                    channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception)
                {
                    channel.BasicNack(ea.DeliveryTag, false, true);
                }
            };

            channel.BasicConsume(_queueName, autoAck: false, consumer);

            cancellationToken.WaitHandle.WaitOne();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _consumer.Received -= OnMessageReceived;

            _channel?.Close();
            _connection?.Close();

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
