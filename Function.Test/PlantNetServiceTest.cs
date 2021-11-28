using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Function.Repository;
using Function.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace Function.Tests
{
    [TestFixture]
    public class PlantNetServiceTest
    {
        private HttpClient CreateHttpClient()
        {
            // Instantiate the mock object
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            // Set up the SendAsync method behavior.
            httpMessageHandlerMock
                .Protected() // <= here is the trick to set up protected!!!
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage());
            // create the HttpClient
            return new HttpClient(httpMessageHandlerMock.Object);
        }

        [Test][Ignore("Not ready")]
        public void PlantNetService_GetLanguage_ReturnsEnIfNoDataFound()
        {
            //Arrange
            var loggerMock = new Mock<ILogger<PlantNetService>>();
            PlantNetService repo = new(CreateHttpClient(), loggerMock.Object);

            // Act
        }
    }
}