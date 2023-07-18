using api.Controllers;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace tests.Controllers
{
    public class WalletControllerTests
    {
        [Fact]
        public void Transact_ReturnsAccepted()
        {
            var request = new TransactionRequest()
            {
                Stock = new StockBase()
                {
                    Code = "1",
                    Amount = 1
                },
                Type = TransactionType.Purchase
            };
            var loggerMock = new Mock<ILogger<WalletController>>();
            var publisherServiceMock = new Mock<IPublisherService>();
            var transactionServiceMock = new Mock<ITransactionService>();
            var walletController = new WalletController(loggerMock.Object, publisherServiceMock.Object, transactionServiceMock.Object);
            var result = walletController.Transact(request);

            publisherServiceMock.Verify(s => s.PublishMessage(request), Times.Once());
            loggerMock.Verify(s => s.Log(
                LogLevel.Information,
                0,
                It.IsAny<It.IsAnyType>(),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once());

            Assert.IsType<AcceptedResult>(result);
            Assert.Equal("Message published.", ((AcceptedResult)result).Location);
        }

        [Fact]
        public void GetWallet_ReturnsWallet()
        {
            var wallet = new Wallet()
            {
                Total = 1,
                Stocks = new List<StockTotal>()
                {
                    new StockTotal()
                    {
                        Code = "1",
                        Amount = 1,
                        Price = 1,
                        Total = 1
                    }
                }
            };
            var loggerMock = new Mock<ILogger<WalletController>>();
            var publisherServiceMock = new Mock<IPublisherService>();
            var transactionServiceMock = new Mock<ITransactionService>();

            transactionServiceMock.Setup(s => s.GetWallet()).Returns(wallet);

            var walletController = new WalletController(loggerMock.Object, publisherServiceMock.Object, transactionServiceMock.Object);
            var result = walletController.GetWallet();

            Assert.Equal(wallet, result);
        }

        [Fact]
        public void GetTransactions_ReturnsTransactions()
        {
            var transactions = new List<Transaction>()
            {
                new Transaction()
                {
                    Id = "",
                    Date = DateTime.UtcNow,
                    Processed = true,
                    StatusMessage = "OK",
                    Stock = new Stock()
                    {
                        Code = "",
                        Amount = 10,
                        Price = 10
                    },
                    Type = TransactionType.Purchase
                }
            };
            var loggerMock = new Mock<ILogger<WalletController>>();
            var publisherServiceMock = new Mock<IPublisherService>();
            var transactionServiceMock = new Mock<ITransactionService>();

            transactionServiceMock.Setup(s => s.GetTransactions()).Returns(transactions);

            var walletController = new WalletController(loggerMock.Object, publisherServiceMock.Object, transactionServiceMock.Object);
            var result = walletController.GetTransactions();

            Assert.Equal(transactions, result);
        }
    }
}
