using api.Models;

namespace api.Services
{
    public class StockService : IStockService
    {
        private static readonly List<Stock> stocks = new()
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
            },
        };

        public bool Create(Stock stock)
        {
            var exists = stocks.Any(s => s.Code == stock.Code);

            if (!exists)
            {
                stocks.Add(stock);

                return true;
            }

            return false;
        }

        public IEnumerable<Stock> Get() => stocks;

        public Stock? Get(string code) => stocks.FirstOrDefault(s => s.Code == code);

        public bool Update(string code, Stock updatedStock)
        {
            var stock = Get(code);

            if (stock is not null)
            {
                stock.Amount = updatedStock.Amount;
                stock.Price = updatedStock.Price;

                return true;
            }

            return false;
        }

        public bool Remove(string code)
        {
            var stock = Get(code);

            if (stock is not null)
            {
                stocks.Remove(stock);

                return true;
            }

            return false;
        }
    }
}
