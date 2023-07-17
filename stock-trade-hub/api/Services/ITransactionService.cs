using api.Models;

namespace api.Services
{
    public interface ITransactionService
    {
        public Transaction Transact(TransactionTemp transaction);

        public Wallet GetWallet();

        public IEnumerable<Transaction> GetTransactions();
    }
}
