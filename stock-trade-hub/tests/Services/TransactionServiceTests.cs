using api.Models;
using api.Services;
using Moq;

namespace tests.Services
{
    public class TransactionServiceTests
    {
        [Fact]
        public void Transact_Purchase_ReturnsTransaction()
        {
            var stockService = new Mock<IStockService>();
            var stock = new Stock()
            {
                Code = "STNE",
                Amount = 1,
                Price = 1
            };

            stockService.Setup(s => s.Get(It.IsAny<string>())).Returns(stock);

            var publisherService = new Mock<IPublisherService>();
            var transactionTemp2 = new TransactionTemp()
            {
                Id = "1",
                Processed = false,
                Stock = new StockBase()
                {
                    Code = "STNE",
                    Amount = 1
                },
                Type = TransactionType.Purchase
            };

            publisherService.Setup(s => s.Get(It.IsAny<string>())).Returns(transactionTemp2);

            var transactionService = new TransactionService(stockService.Object, publisherService.Object);
            var transactionTemp = new TransactionTemp()
            {
                Id = "1",
                Processed = false,
                Stock = new StockBase()
                {
                    Code = "STNE",
                    Amount = 1
                },
                Type = TransactionType.Purchase
            };
            var result = transactionService.Transact(transactionTemp);

            Assert.Equivalent(new Stock()
            {
                Code = "STNE",
                Amount = 1,
                Price = 1
            }, result.Stock);
            Assert.Equal(transactionTemp.Type, result.Type);
            Assert.True(result.Processed);
            Assert.Equal("Transaction processed successfully.", result.StatusMessage);
        }

        [Fact]
        public void Transact_Selling_ReturnsTransaction()
        {
            var stockService = new Mock<IStockService>();
            var stock = new Stock()
            {
                Code = "STNE",
                Amount = 1,
                Price = 1
            };

            stockService.Setup(s => s.Get(It.IsAny<string>())).Returns(stock);

            var publisherService = new Mock<IPublisherService>();
            var transactionTemp2 = new TransactionTemp()
            {
                Id = "1",
                Processed = false,
                Stock = new StockBase()
                {
                    Code = "STNE",
                    Amount = 1
                },
                Type = TransactionType.Selling
            };

            publisherService.Setup(s => s.Get(It.IsAny<string>())).Returns(transactionTemp2);

            var transactionService = new TransactionService(stockService.Object, publisherService.Object);
            var transactionTemp = new TransactionTemp()
            {
                Id = "1",
                Processed = false,
                Stock = new StockBase()
                {
                    Code = "STNE",
                    Amount = 1
                },
                Type = TransactionType.Purchase
            };

            _ = transactionService.Transact(transactionTemp);

            transactionTemp.Type = TransactionType.Selling;
            transactionTemp2.Processed = false;

            var result = transactionService.Transact(transactionTemp);

            Assert.Equivalent(new Stock()
            {
                Code = "STNE",
                Amount = 1,
                Price = 1
            }, result.Stock);
            Assert.Equal(transactionTemp.Type, result.Type);
            Assert.True(result.Processed);
            Assert.Equal("Transaction processed successfully.", result.StatusMessage);
        }

        [Fact]
        public void GetWallet_ReturnsWallet()
        {
            var stockService = new Mock<IStockService>();
            var publisherService = new Mock<IPublisherService>();
            var transactionService = new TransactionService(stockService.Object, publisherService.Object);
            var result = transactionService.GetWallet();

            Assert.Equivalent(new Wallet()
            {
                Total = 0,
                Stocks = new List<StockTotal>()
            }, result);
        }

        [Fact]
        public void GetTransactions_ReturnsEmpty()
        {
            var stockService = new Mock<IStockService>();
            var publisherService = new Mock<IPublisherService>();
            var transactionService = new TransactionService(stockService.Object, publisherService.Object);
            var result = transactionService.GetTransactions();

            Assert.Empty(result);
        }
    }
}
