using Moq;
using Moq.Protected;
using OrderService.API.Exceptions;
using OrderService.API.Services;
using System.Net;
using System.Text;
using Xunit;

namespace OrderService.Tests.Services
{
    public class ProductServiceClientTests
    {
        [Fact]
        public async Task GetProductAsync_ShouldReturnProduct()
        {
            // Arrange

            var response =
                """
                {
                    "data":
                    {
                        "id":1,
                        "name":"Laptop",
                        "price":50000,
                        "stock":10
                    }
                }
                """;

            var handlerMock =
                new Mock<HttpMessageHandler>();

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,

                    Content = new StringContent(
                        response,
                        Encoding.UTF8,
                        "application/json")
                });

            var httpClient =
                new HttpClient(handlerMock.Object);

            httpClient.BaseAddress =
                new Uri("https://localhost:7210");

            var service =
                new ProductServiceClient(httpClient);

            // Act

            var result =
                await service.GetProductAsync(
                    1,
                    "token");

            // Assert

            Assert.NotNull(result);

            Assert.Equal(
                "Laptop",
                result!.Name);
        }

        [Fact]
        public async Task GetProductAsync_ShouldReturnNull_WhenProductNotFound()
        {
            // Arrange

            var handlerMock =
                new Mock<HttpMessageHandler>();

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

            var httpClient =
                new HttpClient(handlerMock.Object);

            httpClient.BaseAddress =
                new Uri("https://localhost:7210");

            var service =
                new ProductServiceClient(httpClient);

            // Act

            var result =
                await service.GetProductAsync(
                    1,
                    "token");

            // Assert

            Assert.Null(result);
        }

        [Fact]
        public async Task DeductStockAsync_ShouldThrowException_WhenApiFails()
        {
            // Arrange

            var handlerMock =
                new Mock<HttpMessageHandler>();

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,

                    Content = new StringContent(
                        "Stock error")
                });

            var httpClient =
                new HttpClient(handlerMock.Object);

            httpClient.BaseAddress =
                new Uri("https://localhost:7210");

            var service =
                new ProductServiceClient(httpClient);

            // Act & Assert

            await Assert.ThrowsAsync<ValidationException>(
                () => service.DeductStockAsync(
                    1,
                    2,
                    "token"));
        }

        [Fact]
        public async Task RestockProductAsync_ShouldThrowException_WhenApiFails()
        {
            // Arrange

            var handlerMock =
                new Mock<HttpMessageHandler>();

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,

                    Content = new StringContent(
                        "Restock error")
                });

            var httpClient =
                new HttpClient(handlerMock.Object);

            httpClient.BaseAddress =
                new Uri("https://localhost:7210");

            var service =
                new ProductServiceClient(httpClient);

            // Act & Assert

            await Assert.ThrowsAsync<ValidationException>(
                () => service.RestockProductAsync(
                    1,
                    2,
                    "token"));
        }
    }
}