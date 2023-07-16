using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WalletController : ControllerBase
    {
        private readonly ILogger<WalletController> _logger;
        private readonly IMessageService _messageService;
        private readonly ITransactionService _transactionService;

        public WalletController(
            ILogger<WalletController> logger,
            IMessageService messageService,
            ITransactionService transactionService)
        {
            _logger = logger;
            _messageService = messageService;
            _transactionService = transactionService;
        }

        [HttpPost(Name = "CreateTransaction")]
        public ActionResult<Stock> Transact(TransactionRequest transaction)
        {
            _messageService.PublishMessage(transaction);

            return Ok("Message published.");
        }

        [HttpGet(Name = "GetWallet")]
        public IEnumerable<StockBase> GetWallet() => _transactionService.GetWallet();

        [HttpGet(Name = "GetTransactions")]
        public IEnumerable<Transaction> GetTransactions() => _transactionService.GetTransactions();
    }
}
