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
            stock.Code = stock.Code.Trim().ToUpper();

            var exists = stocks.Any(s => s.Code == stock.Code);

            if (!exists)
            {
                stocks.Add(stock);

                return true;
            }

            return false;
        }

        public IEnumerable<Stock> Get() => stocks;

        public Stock? Get(string code) => stocks.FirstOrDefault(s => s.Code == code.Trim().ToUpper());

        public bool Update(string code, StockUpdate updatedStock)
        {
            var stock = Get(code);

            if (stock is not null)
            {
                if (updatedStock.Code is not null)
                {
                    stock.Code = updatedStock.Code.Trim().ToLower();
                }

                if (updatedStock.Amount is not null)
                {
                    stock.Amount = (int)updatedStock.Amount;
                }

                if (updatedStock.Price is not null)
                {
                    stock.Price = updatedStock.Price;
                }

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
