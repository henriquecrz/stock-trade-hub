using api.Models;
using api.Utils;

namespace api.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly List<StockBase> _wallet;
        private readonly List<Transaction> _transactions;
        private readonly IStockService _stockService;
        private readonly IPublisherService _publisherService;

        public TransactionService(IStockService stockService, IPublisherService publisherService)
        {
            _wallet = new();
            _transactions = new();
            _stockService = stockService;
            _publisherService = publisherService;
        }

        public Transaction Transact(TransactionTemp transactionTemp)
        {
            var transactionStock = transactionTemp.Stock;
            var stock = _stockService.Get(transactionStock.Code);

            if (stock is null)
            {
                return CreateTransaction(transactionTemp, processed: false, $"Stock code {transactionStock.Code} does not exist."); //BadRequest
            }

            if (!transactionStock.IsValid)
            {
                return CreateTransaction(transactionTemp, processed: false, $"The amount can not be {transactionStock.Amount}."); //BadRequest
            }

            var transactionTemp2 = _publisherService.Get(transactionTemp.Id);

            if (transactionTemp2 is not null && !transactionTemp2.Processed)
            {
                return transactionTemp.Type switch
                {
                    TransactionType.Purchase => Purchase(transactionTemp, stock, transactionTemp2),
                    TransactionType.Selling => Selling(transactionTemp, stock, transactionTemp2),
                    _ => Default(transactionTemp)
                };
            }

            return CreateTransaction(transactionTemp, processed: false, $"The request {transactionTemp.Id} was already processed."); //BadRequest
        }

        public Wallet GetWallet()
        {
            var stocks = _wallet.Select(walletStock =>
            {
                var stock = _stockService.Get(walletStock.Code);
                var amount = walletStock.Amount;
                var price = stock?.Price;

                return new StockTotal()
                {
                    Code = walletStock.Code,
                    Amount = amount,
                    Price = price,
                    Total = amount * price
                };
            });
            var total = stocks.Aggregate((decimal)0, (acc, entry) => acc + entry.Total ?? default);

            return new Wallet()
            {
                Total = total,
                Stocks = stocks
            };
        }

        public IEnumerable<Transaction> GetTransactions() => _transactions;

        private Transaction Purchase(TransactionTemp transactionTemp, Stock stock, TransactionTemp transactionTemp2)
        {
            var transactionStock = transactionTemp.Stock;

            if (transactionStock.Amount <= stock.Amount)
            {
                var walletStock = _wallet.GetStock(transactionStock.Code);

                if (walletStock is null)
                {
                    _wallet.Add(new StockBase()
                    {
                        Code = transactionStock.Code,
                        Amount = transactionStock.Amount
                    });
                }
                else
                {
                    walletStock.Amount += transactionStock.Amount;
                }

                stock.Amount -= transactionStock.Amount;
                transactionTemp2.Processed = true;

                var processed = CreateTransaction(transactionTemp, processed: true, "Transaction processed successfully.");

                _transactions.Add(processed);

                return processed; //CreatedAtAction
            }

            return CreateTransaction(
                transactionTemp,
                processed: false,
                string.Format(
                    "The number of stocks to be bought ({0}) is greater than the available ({1}).",
                    transactionStock.Amount,
                    stock.Amount)); //BadRequest
        }

        private Transaction Selling(TransactionTemp transactionTemp, Stock stock, TransactionTemp transactionTemp2)
        {
            var transactionStock = transactionTemp.Stock;
            var walletStock = _wallet.GetStock(transactionStock.Code);

            if (walletStock is not null)
            {
                if (transactionStock.Amount <= walletStock.Amount)
                {
                    walletStock.Amount -= transactionStock.Amount;
                    stock.Amount += transactionStock.Amount;

                    if (walletStock.Amount == 0)
                    {
                        _wallet.Remove(walletStock);
                    }

                    transactionTemp2.Processed = true;

                    var processed = CreateTransaction(transactionTemp, processed: true, "Transaction processed successfully.");

                    _transactions.Add(processed);

                    return processed; //CreatedAtAction
                }

                return CreateTransaction(transactionTemp, processed: false, "You can't sell a stock you don't have in the wallet."); //BadRequest
            }

            return CreateTransaction(transactionTemp, processed: false, "You can't sell a stock you don't have in the wallet."); //BadRequest
        }

        private Transaction Default(TransactionTemp transaction) =>
            CreateTransaction(transaction, processed: false, $"Invalid transaction type ({transaction.Type})."); //BadRequest

        private Transaction CreateTransaction(TransactionTemp transaction, bool processed, string statusMessage)
        {
            var transactionStock = transaction.Stock;
            var stock = _stockService.Get(transactionStock.Code);

            return new()
            {
                Id = Guid.NewGuid().ToString(),
                Stock = new Stock()
                {
                    Code = transactionStock.Code,
                    Amount = transactionStock.Amount,
                    Price = stock?.Price
                },
                Type = transaction.Type,
                Date = DateTime.UtcNow,
                Processed = processed,
                StatusMessage = statusMessage
            };
        }
    }
}
