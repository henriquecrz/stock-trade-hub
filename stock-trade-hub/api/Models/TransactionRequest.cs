namespace api.Models
{
    public class TransactionRequest
    {
        public required StockBase Stock { get; set; }

        public required TransactionType Type { get; set; }
    }
}
