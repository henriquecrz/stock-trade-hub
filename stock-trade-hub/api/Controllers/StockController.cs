using api.Models;
using api.Utils;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class StockController : ControllerBase
    {
        public static readonly List<Stock> Stocks = new()
        {
            new Stock
            {
                Code = "STNE",
                Amount = 100,
                Price = 69.33m
            },
            new Stock
            {
                Code = "GOOG",
                Amount = 100,
                Price = 117.71m
            },
            new Stock
            {
                Code = "PGRM",
                Amount = 100,
                Price = 132.17m
            },
        };

        private readonly ILogger<StockController> _logger;

        public StockController(ILogger<StockController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "CreateStock")]
        public ActionResult<Stock> Create(Stock stock)
        {
            var exists = Stocks.Any(s => s.Code == stock.Code);

            if (!exists)
            {
                Stocks.Add(stock);

                return CreatedAtAction(nameof(Get), new { stock.Code }, stock);
            }

            return BadRequest($"Stock code {stock.Code} already exists.");
        }

        [HttpGet(Name = "GetStocks")]
        public IEnumerable<Stock> Get()
        {
            return Stocks;
        }

        [HttpGet(Name = "GetStockByCode")]
        public ActionResult<Stock> GetByCode(string code)
        {
            code = code.Trim().ToUpper();
            var stock = Stocks.GetStock(code);

            if (stock is not null)
            {
                return stock;
            }

            return NotFound($"Stock code {code} not found.");
        }

        [HttpPut(Name = "UpdateStock")]
        public ActionResult<Stock> Update(string code, Stock updatedStock)
        {
            code = code.Trim().ToUpper();
            var stock = Stocks.GetStock(code);

            if (stock is not null)
            {
                stock.Amount = updatedStock.Amount;
                stock.Price = updatedStock.Price;

                return Ok(stock);
            }

            return NotFound($"Stock code {code} not found.");
        }

        [HttpDelete(Name = "DeleteStock")]
        public ActionResult<Stock> Delete(string code)
        {
            code = code.Trim().ToUpper();
            var stock = GetByCode(code).Value;

            if (stock is not null)
            {
                Stocks.Remove(stock);

                return Ok(stock);
            }

            return NotFound($"Stock code {code} not found.");
        }
    }
}
