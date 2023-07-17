using api.Models;

namespace api.Services
{
    public interface ITransactionService
    {
        public Transaction Transact(TransactionRequest transaction);

        public Wallet GetWallet();

        public IEnumerable<Transaction> GetTransactions();
    }
}
