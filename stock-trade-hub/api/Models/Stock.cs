using System.Text.Json.Serialization;

namespace api.Models
{
    public class Stock : StockBase
    {
        public decimal? Price { get; set; }

        [JsonIgnore]
        public override bool IsValid =>
            Code.Length > 0 &&
            Amount >= 0 &&
            Price > 0;
    }
}
