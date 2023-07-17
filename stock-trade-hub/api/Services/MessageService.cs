﻿using api.Models;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace api.Services
{
    public class MessageService : IMessageService
    {
        private readonly ILogger<MessageService> _logger;
        private readonly ConnectionFactory _connectionFactory;

        public MessageService(ILogger<MessageService> logger)
        {
            _logger = logger;
            _connectionFactory = new ConnectionFactory() { HostName = "localhost" };
        }

        public void PublishMessage(TransactionRequest transaction)
        {
            using var connection = _connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            var message = JsonSerializer.Serialize(transaction);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "", routingKey: Utils.Constants.QUEUE_NAME, basicProperties: null, body: body);

            _logger.LogInformation($"Message published: {message}");
        }
    }
}
