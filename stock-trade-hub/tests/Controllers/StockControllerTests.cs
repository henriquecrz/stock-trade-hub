using api.Controllers;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;

namespace tests.Controllers
{
    public class StockControllerTests
    {
        [Fact]
        public void Create_ReturnsCreatedAtAction()
        {
            var stock = new Stock()
            {
                Code = "1",
                Amount = 1,
                Price = 1
            };
            var loggerMock = new Mock<ILogger<StockController>>();
            var stockServiceMock = new Mock<IStockService>();

            stockServiceMock.Setup(s => s.Create(stock)).Returns(true);

            var stockController = new StockController(loggerMock.Object, stockServiceMock.Object);
            var result = stockController.Create(stock).Result;

            stockServiceMock.Verify(s => s.Create(stock), Times.Once());

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);

            Assert.Equal(201, createdAtActionResult.StatusCode);
            Assert.Equal("Get", createdAtActionResult.ActionName);
            Assert.Equal(new RouteValueDictionary(new { Code = "1" }), createdAtActionResult.RouteValues);
            Assert.Equal(stock, createdAtActionResult.Value);
        }

        [Fact]
        public void Create_ReturnsBadRequest()
        {
            var stock = new Stock()
            {
                Code = "1",
                Amount = 1,
                Price = 1
            };
            var loggerMock = new Mock<ILogger<StockController>>();
            var stockServiceMock = new Mock<IStockService>();

            stockServiceMock.Setup(s => s.Create(stock)).Returns(false);

            var stockController = new StockController(loggerMock.Object, stockServiceMock.Object);
            var result = stockController.Create(stock).Result;

            stockServiceMock.Verify(s => s.Create(stock), Times.Once());

            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(400, badRequestObjectResult.StatusCode);
            Assert.Equal("Stock code 1 already exists or it is invalid.", badRequestObjectResult.Value);
        }

        [Fact]
        public void Get_ReturnsStocks()
        {
            var stocks = new List<Stock>()
            {
                new Stock()
                {
                    Code = "1",
                    Amount = 1,
                    Price = 1
                }
            };
            var loggerMock = new Mock<ILogger<StockController>>();
            var stockServiceMock = new Mock<IStockService>();

            stockServiceMock.Setup(s => s.Get()).Returns(stocks);

            var stockController = new StockController(loggerMock.Object, stockServiceMock.Object);
            var result = stockController.Get();

            stockServiceMock.Verify(s => s.Get(), Times.Once());

            Assert.Equal(stocks, result);
        }

        [Fact]
        public void GetByCode_ReturnsStock()
        {
            var code = " a ";
            var stock = new Stock()
            {
                Code = "A",
                Amount = 1,
                Price = 1
            };
            var loggerMock = new Mock<ILogger<StockController>>();
            var stockServiceMock = new Mock<IStockService>();

            stockServiceMock.Setup(s => s.Get(It.IsAny<string>())).Returns(stock);

            var stockController = new StockController(loggerMock.Object, stockServiceMock.Object);
            var result = stockController.GetByCode(code).Value;

            stockServiceMock.Verify(s => s.Get("A"), Times.Once());

            Assert.Equal(stock, result);
        }

        [Fact]
        public void GetByCode_ReturnsNotFound()
        {
            var code = " a ";
            Stock? stock = null;
            var loggerMock = new Mock<ILogger<StockController>>();
            var stockServiceMock = new Mock<IStockService>();

            stockServiceMock.Setup(s => s.Get(It.IsAny<string>())).Returns(stock);

            var stockController = new StockController(loggerMock.Object, stockServiceMock.Object);
            var result = stockController.GetByCode(code).Result;

            stockServiceMock.Verify(s => s.Get("A"), Times.Once());

            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);

            Assert.Equal(404, notFoundObjectResult.StatusCode);
            Assert.Equal("Stock code A not found.", notFoundObjectResult.Value);
        }

        [Fact]
        public void Update_ReturnsUpdatedStock()
        {
            var code = " a ";
            var updatedStock = new StockUpdate()
            {
                Code = "A",
                Amount = 1,
                Price = 1
            };
            var loggerMock = new Mock<ILogger<StockController>>();
            var stockServiceMock = new Mock<IStockService>();

            stockServiceMock.Setup(s => s.Update(It.IsAny<string>(), It.IsAny<StockUpdate>())).Returns(true);

            var stockController = new StockController(loggerMock.Object, stockServiceMock.Object);
            var result = stockController.Update(code, updatedStock).Result;

            stockServiceMock.Verify(s => s.Update("A", updatedStock), Times.Once());

            var okObjectResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.Equal(updatedStock, okObjectResult.Value);
        }

        [Fact]
        public void Update_ReturnsNotFound()
        {
            var code = " a ";
            var updatedStock = new StockUpdate()
            {
                Code = "A",
                Amount = 1,
                Price = 1
            };
            var loggerMock = new Mock<ILogger<StockController>>();
            var stockServiceMock = new Mock<IStockService>();

            stockServiceMock.Setup(s => s.Update(It.IsAny<string>(), It.IsAny<StockUpdate>())).Returns(false);

            var stockController = new StockController(loggerMock.Object, stockServiceMock.Object);
            var result = stockController.Update(code, updatedStock).Result;

            stockServiceMock.Verify(s => s.Update("A", updatedStock), Times.Once());

            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);

            Assert.Equal(404, notFoundObjectResult.StatusCode);
            Assert.Equal("Stock code A not found.", notFoundObjectResult.Value);
        }

        [Fact]
        public void Delete_ReturnsStockCodeDeleted()
        {
            var code = " a ";
            var loggerMock = new Mock<ILogger<StockController>>();
            var stockServiceMock = new Mock<IStockService>();

            stockServiceMock.Setup(s => s.Remove(It.IsAny<string>())).Returns(true);

            var stockController = new StockController(loggerMock.Object, stockServiceMock.Object);
            var result = stockController.Delete(code).Result;

            stockServiceMock.Verify(s => s.Remove("A"), Times.Once());

            var okObjectResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.Equal("A", okObjectResult.Value);
        }

        [Fact]
        public void Delete_ReturnsNotFound()
        {
            var code = " a ";
            var loggerMock = new Mock<ILogger<StockController>>();
            var stockServiceMock = new Mock<IStockService>();

            stockServiceMock.Setup(s => s.Remove(It.IsAny<string>())).Returns(false);

            var stockController = new StockController(loggerMock.Object, stockServiceMock.Object);
            var result = stockController.Delete(code).Result;

            stockServiceMock.Verify(s => s.Remove("A"), Times.Once());

            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);

            Assert.Equal(404, notFoundObjectResult.StatusCode);
            Assert.Equal("Stock code A not found.", notFoundObjectResult.Value);
        }
    }
}
