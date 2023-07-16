using api.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace api.Services
{
    public class MessageService : IMessageService
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly string _queueName;
        private readonly ILogger<MessageService> _logger;
        private readonly ITransactionService _transactionService;

        public MessageService(
            ILogger<MessageService> logger,
            ITransactionService transactionService)
        {
            _connectionFactory = new ConnectionFactory() { HostName = "localhost" };
            _queueName = "transaction";
            _logger = logger;
            _transactionService = transactionService;

            ConsumeMessages();
        }

        public void PublishMessage(TransactionRequest transaction)
        {
            using var connection = _connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(_queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var message = JsonSerializer.Serialize(transaction);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);

            Console.WriteLine("Message published: {0}", message);
        }

        private void ConsumeMessages()
        {
            using var connection = _connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
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
        }
    }
}
