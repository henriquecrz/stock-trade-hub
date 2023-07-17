using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class StockController : ControllerBase
    {
        private readonly ILogger<StockController> _logger;
        private readonly IStockService _stockService;

        public StockController(ILogger<StockController> logger, IStockService stockService)
        {
            _logger = logger;
            _stockService = stockService;
        }

        [HttpPost(Name = "CreateStock")]
        public ActionResult<Stock> Create(Stock stock)
        {
            var success = _stockService.Create(stock);

            if (success)
            {
                return CreatedAtAction(nameof(Get), new { stock.Code }, stock);
            }

            return BadRequest($"Stock code {stock.Code} already exists.");
        }

        [HttpGet(Name = "GetStocks")]
        public IEnumerable<Stock> Get() => _stockService.Get();

        [HttpGet(Name = "GetStockByCode")]
        public ActionResult<Stock> GetByCode(string code)
        {
            var stock = _stockService.Get(code);

            if (stock is not null)
            {
                return stock;
            }

            return NotFound($"Stock code {code} not found.");
        }

        [HttpPut(Name = "UpdateStock")]
        public ActionResult<Stock> Update(string code, StockUpdate updatedStock)
        {
            var success = _stockService.Update(code, updatedStock);

            if (success)
            {
                return Ok(updatedStock);
            }

            return NotFound($"Stock code {code} not found.");
        }

        [HttpDelete(Name = "DeleteStock")]
        public ActionResult<Stock> Delete(string code)
        {
            var success = _stockService.Remove(code);

            if (success)
            {
                return Ok(code);
            }

            return NotFound($"Stock code {code} not found.");
        }
    }
}
