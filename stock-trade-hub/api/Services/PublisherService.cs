using api.Models;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace api.Services
{
    public class PublisherService : IPublisherService
    {
        private readonly ILogger<PublisherService> _logger;
        private readonly ConnectionFactory _connectionFactory;
        private readonly List<TransactionTemp> _transactions;

        public PublisherService(ILogger<PublisherService> logger)
        {
            _logger = logger;
            _connectionFactory = new ConnectionFactory() { HostName = "localhost" };
            _transactions = new();
        }

        public void PublishMessage(TransactionRequest request)
        {
            using var connection = _connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            var transaction = new TransactionTemp()
            {
                Id = Guid.NewGuid().ToString(),
                Stock = request.Stock,
                Type = request.Type,
                Processed = false
            };
            var message = JsonSerializer.Serialize(transaction);
            var body = Encoding.UTF8.GetBytes(message);

            _transactions.Add(transaction);

            channel.BasicPublish(exchange: string.Empty, Utils.Constants.QUEUE_NAME, basicProperties: null, body);

            _logger.LogInformation($"Message published: {message}");
        }

        public TransactionTemp? Get(string id) => _transactions.FirstOrDefault(t => t.Id == id);
    }
}
