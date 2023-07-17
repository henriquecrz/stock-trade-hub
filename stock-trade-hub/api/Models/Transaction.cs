namespace api.Models
{
    public class Transaction
    {
        public required string? Id { get; set; }

        public required Stock Stock { get; set; }

        public required TransactionType Type { get; set; }

        public required DateTime Date { get; set; }

        public required bool Processed { get; set; }

        public required string StatusMessage { get; set; }
    }
}
