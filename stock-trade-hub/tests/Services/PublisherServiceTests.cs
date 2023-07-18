using api.Models;
using api.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace tests.Services
{
    public class PublisherServiceTests
    {
        [Fact]
        public void PublishMessage_PublishesInQueue()
        {
            var request = new TransactionRequest()
            {
                Stock = new StockBase()
                {
                    Code = " stne ",
                    Amount = 1
                },
                Type = TransactionType.Purchase
            };
            var loggerMock = new Mock<ILogger<PublisherService>>();
            var publisherService = new PublisherService(loggerMock.Object);

            publisherService.PublishMessage(request);

            loggerMock.Verify(s => s.Log(
                LogLevel.Information,
                0,
                It.IsAny<It.IsAnyType>(),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once());
        }

        [Fact]
        public void Get_ReturnsNull()
        {
            var id = "1";
            var loggerMock = new Mock<ILogger<PublisherService>>();
            var publisherService = new PublisherService(loggerMock.Object);
            var result = publisherService.Get(id);

            Assert.Null(result);
        }
    }
}
