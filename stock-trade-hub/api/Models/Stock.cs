namespace api.Models
{
    public class Stock : StockBase
    {
        public required decimal? Price { get; set; }
    }
}
