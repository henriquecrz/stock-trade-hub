using api.Models;

namespace api.Services
{
    public interface IStockService
    {
        public bool Create(Stock stock);

        public IEnumerable<Stock> Get();

        public Stock? Get(string code);

        public bool Update(string code, StockUpdate updatedStock);

        public bool Remove(string code);
    }
}
