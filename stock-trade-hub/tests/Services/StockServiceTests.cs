using api.Models;
using api.Services;

namespace tests.Services
{
    public class StockServiceTests
    {
        [Fact]
        public void Create_ReturnsTrue()
        {
            var stock = new Stock()
            {
                Code = " a ",
                Amount = 1,
                Price = 1
            };
            var stockService = new StockService();
            var result = stockService.Create(stock);

            Assert.True(result);

            var stock2 = stockService.Get("A");

            Assert.Equivalent(new Stock()
            {
                Code = "A",
                Amount = 1,
                Price = 1
            }, stock2);
        }

        [Fact]
        public void Create_ReturnsFalse()
        {
            var stock = new Stock()
            {
                Code = " a ",
                Amount = 1,
                Price = 0
            };
            var stockService = new StockService();
            var result = stockService.Create(stock);

            Assert.False(result);

            var stock2 = stockService.Get("A");

            Assert.Null(stock2);
        }

        [Fact]
        public void Get_ReturnsStocks()
        {
            var stocks = new List<Stock>()
            {
                new Stock
                {
                    Code = "STNE",
                    Amount = 100,
                    Price = 69.33m
                },
                new Stock
                {
                    Code = "GOOG",
                    Amount = 100,
                    Price = 117.71m
                },
                new Stock
                {
                    Code = "PGRM",
                    Amount = 100,
                    Price = 132.17m
                }
            };
            var stockService = new StockService();
            var result = stockService.Get();

            Assert.Equivalent(stocks, result);
        }

        [Fact]
        public void Get_ReturnsStock()
        {
            var stock = new Stock
            {
                Code = "STNE",
                Amount = 100,
                Price = 69.33m
            };
            var stockService = new StockService();
            var result = stockService.Get(stock.Code);

            Assert.Equivalent(stock, result);
        }

        [Fact]
        public void Update_ReturnsTrue()
        {
            var code = "STNE";
            var stock = new StockUpdate()
            {
                Code = "STNE",
                Amount = 1,
                Price = 2
            };
            var stockService = new StockService();
            var result = stockService.Update(code, stock);

            Assert.True(result);

            var stock2 = stockService.Get(code);

            Assert.Equivalent(stock, stock2);
        }

        [Fact]
        public void Update_ReturnsFalse()
        {
            var code = "a";
            var stock = new StockUpdate()
            {
                Code = "STNE",
                Amount = 1,
                Price = 2
            };
            var stockService = new StockService();
            var result = stockService.Update(code, stock);

            Assert.False(result);

            var stock2 = stockService.Get(code);

            Assert.Null(stock2);
        }

        [Fact]
        public void Remove_ReturnsTrue()
        {
            var code = "STNE";
            var stockService = new StockService();
            var result = stockService.Remove(code);

            Assert.True(result);

            var stock = stockService.Get(code);

            Assert.Null(stock);
        }

        [Fact]
        public void Remove_ReturnsFalse()
        {
            var code = "a";
            var stockService = new StockService();
            var result = stockService.Remove(code);

            Assert.False(result);

            var stock = stockService.Get(code);

            Assert.Null(stock);
        }
    }
}
