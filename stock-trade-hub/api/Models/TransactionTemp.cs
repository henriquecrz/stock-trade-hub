namespace api.Models
{
    public class TransactionTemp : TransactionRequest
    {
        public required string Id { get; set; }

        public required bool Processed { get; set; }
    }
}
