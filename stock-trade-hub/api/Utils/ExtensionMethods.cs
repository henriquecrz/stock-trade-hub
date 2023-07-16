using api.Models;

namespace api.Utils
{
    public static class ExtensionMethods
    {
        public static StockBase? GetStock(this List<StockBase> list, string code) =>
            list.FirstOrDefault(s => s.Code == code);

        public static Stock? GetStock(this List<Stock> list, string code) =>
            list.FirstOrDefault(s => s.Code == code);
    }
}
