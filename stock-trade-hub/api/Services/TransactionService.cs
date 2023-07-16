using api.Controllers;
using api.Models;
using api.Utils;

namespace api.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly List<StockBase> wallet;
        private readonly List<Transaction> transactions;

        public TransactionService()
        {
            wallet = new();
            transactions = new();
        }

        public Transaction Transact(TransactionRequest request)
        {
            var transactionStock = request.Stock;
            var stock = StockController.Stocks.GetStock(transactionStock.Code);

            if (stock is null)
            {
                return $"Stock code {transactionStock.Code} does not exist."; //BadRequest
            }

            switch (request.Type)
            {
                case TransactionType.Purchase:
                    {
                        if (transactionStock.Amount <= stock.Amount)
                        {
                            var walletStock = wallet.GetStock(transactionStock.Code);

                            if (walletStock is null)
                            {
                                wallet.Add(new StockBase()
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

                            transactions.Add(transaction);

                            return transaction; //CreatedAtAction
                        }

                        return string.Format(
                            $"The number of stocks to be bought (0) is greater than the available (1).",
                            transactionStock.Amount,
                            stock.Amount); //BadRequest
                    }
                case TransactionType.Selling:
                    {
                        var walletStock = wallet.GetStock(transactionStock.Code);

                        if (walletStock is not null)
                        {
                            walletStock.Amount -= transactionStock.Amount;
                            stock.Amount += transactionStock.Amount;

                            if (walletStock.Amount == 0)
                            {
                                wallet.Remove(walletStock);
                            }

                            var transaction = CreateTransaction(request, processed: true, "Transaction processed successfully.");

                            transactions.Add(transaction);

                            return transaction; //CreatedAtAction
                        }

                        return string.Format(
                            $"You can't sell a stock you don't have in the wallet.",
                            transactionStock.Amount,
                            stock.Amount); //BadRequest
                    }
                default:
                    return $"Invalid transaction type ({request.Type})."; //BadRequest
            }
        }

        public IEnumerable<StockBase> GetWallet() => wallet;

        public IEnumerable<Transaction> GetTransactions() => transactions;

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
