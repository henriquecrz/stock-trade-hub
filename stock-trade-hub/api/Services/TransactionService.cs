using api.Models;
using api.Utils;

namespace api.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly List<StockBase> _wallet;
        private readonly List<Transaction> _transactions;
        private readonly IStockService _stockService;

        public TransactionService(IStockService stockService)
        {
            _wallet = new();
            _transactions = new();
            _stockService = stockService;
        }

        public Transaction Transact(TransactionRequest request)
        {
            var transactionStock = request.Stock;

            var stock = _stockService.Get(transactionStock.Code);

            if (stock is null)
            {
                return CreateTransaction(request, processed: false, $"Stock code {transactionStock.Code} does not exist."); //BadRequest
            }

            if (!transactionStock.IsValid)
            {
                return CreateTransaction(request, processed: false, $"The amount can not be {transactionStock.Amount}."); //BadRequest
            }

            switch (request.Type)
            {
                case TransactionType.Purchase:
                    {
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
                                stock.Amount -= transactionStock.Amount;
                            }

                            var transaction = CreateTransaction(request, processed: true, "Transaction processed successfully.");

                            _transactions.Add(transaction);

                            return transaction; //CreatedAtAction
                        }

                        return CreateTransaction(
                            request,
                            processed: false,
                            string.Format(
                                $"The number of stocks to be bought (0) is greater than the available (1).",
                                transactionStock.Amount,
                                stock.Amount)); //BadRequest
                    }
                case TransactionType.Selling:
                    {
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

                                var transaction = CreateTransaction(request, processed: true, "Transaction processed successfully.");

                                _transactions.Add(transaction);

                                return transaction; //CreatedAtAction
                            }

                            return CreateTransaction(request, processed: false, "You can't sell a stock you don't have in the wallet."); //BadRequest
                        }

                        return CreateTransaction(request, processed: false, "You can't sell a stock you don't have in the wallet."); //BadRequest
                    }
                default:
                    {
                        return CreateTransaction(request, processed: false, $"Invalid transaction type ({request.Type})."); //BadRequest
                    }
            }
        }

        public IEnumerable<StockBase> GetWallet() => _wallet;

        public IEnumerable<Transaction> GetTransactions() => _transactions;

        private static Transaction CreateTransaction(TransactionRequest request, bool processed, string statusMessage) =>
            new()
            {
                Id = Guid.NewGuid().ToString(),
                Request = request,
                Date = DateTime.UtcNow,
                Processed = processed,
                StatusMessage = statusMessage
            };
    }
}
