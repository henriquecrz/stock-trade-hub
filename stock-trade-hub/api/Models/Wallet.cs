namespace api.Models
{
    public class Wallet
    {
        public required decimal Total { get; set; }

        public required IEnumerable<StockTotal> Stocks { get; set; }
    }
}
