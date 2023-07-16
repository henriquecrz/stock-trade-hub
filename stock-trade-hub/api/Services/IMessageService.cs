using api.Models;

namespace api.Services
{
    public interface IMessageService
    {
        public void PublishMessage(TransactionRequest transaction);
    }
}
