using System.Text.Json.Serialization;

namespace api.Models
{
    public class StockBase
    {
        public required string Code { get; set; }

        public required int Amount { get; set; }

        [JsonIgnore]
        public bool IsValid => Amount > 0;
    }
}
