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
        private readonly IPublisherService _publisherService;
        private readonly ITransactionService _transactionService;

        public WalletController(
            ILogger<WalletController> logger,
            IPublisherService publisherService,
            ITransactionService transactionService)
        {
            _logger = logger;
            _publisherService = publisherService;
            _transactionService = transactionService;
        }

        [HttpPost(Name = "CreateTransaction")]
        public ActionResult<Stock> Transact(TransactionRequest transaction)
        {
            _publisherService.PublishMessage(transaction);

            _logger.LogInformation("Transaction request published.");

            return Accepted("Message published.");
        }

        [HttpGet(Name = "GetWallet")]
        public Wallet GetWallet() => _transactionService.GetWallet();

        [HttpGet(Name = "GetTransactions")]
        public IEnumerable<Transaction> GetTransactions() => _transactionService.GetTransactions();
    }
}
