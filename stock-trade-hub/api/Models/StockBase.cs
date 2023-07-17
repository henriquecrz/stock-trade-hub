namespace api.Models
{
    public class StockBase
    {
        private string? _code;

        public required string? Code
        {
            get => _code;
            set
            {
                _code = _code?.ToUpper();
            }
        }

        public required int Amount { get; set; }

        public bool IsValid => Amount > 0;
    }
}
