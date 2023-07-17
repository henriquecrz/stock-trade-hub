﻿using api.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Channels;
using Constants = api.Utils.Constants;

namespace api.Services
{
    public class ConsumerService : IHostedService, IDisposable
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly ILogger<ConsumerService> _logger;
        private readonly ITransactionService _transactionService;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public ConsumerService(ILogger<ConsumerService> logger, ITransactionService transactionService)
        {
            _connectionFactory = new ConnectionFactory() { HostName = "localhost" };
            _logger = logger;
            _transactionService = transactionService;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(() => ConsumeMessages(_cancellationTokenSource.Token));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource.Cancel();

            //_channel?.Close();
            //_connection?.Close();

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            //_channel?.Dispose();
            //_connection?.Dispose();
        }

        private void ConsumeMessages(CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(Constants.QUEUE_NAME, durable: false, exclusive: false, autoDelete: false, arguments: null);

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

            channel.BasicConsume(Constants.QUEUE_NAME, autoAck: false, consumer);

            while (!cancellationToken.IsCancellationRequested)
            {
                // Aguarda um pequeno intervalo antes de verificar novamente se a tarefa foi cancelada
                Task.Delay(TimeSpan.FromSeconds(1), cancellationToken).Wait();
            }
        }
    }
}