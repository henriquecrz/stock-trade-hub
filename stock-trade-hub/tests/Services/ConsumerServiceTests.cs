using api.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace tests.Services
{
    public class ConsumerServiceTests
    {
        [Fact]
        public void StartAsync_ReturnsTask()
        {
            var logger = new Mock<ILogger<ConsumerService>>();
            var transactionService = new Mock<ITransactionService>();
            var consumerService = new ConsumerService(logger.Object, transactionService.Object);
            var result = consumerService.StartAsync(CancellationToken.None);

            Assert.Equal(TaskStatus.RanToCompletion, result.Status);
        }

        [Fact]
        public void StopAsync_ReturnsTask()
        {
            var logger = new Mock<ILogger<ConsumerService>>();
            var transactionService = new Mock<ITransactionService>();
            var consumerService = new ConsumerService(logger.Object, transactionService.Object);
            var result = consumerService.StopAsync(CancellationToken.None);

            Assert.Equal(TaskStatus.RanToCompletion, result.Status);
        }
    }
}
