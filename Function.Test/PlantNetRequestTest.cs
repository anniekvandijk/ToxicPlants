using System;
using System.Net;
using Function.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Function.Tests
{
    [TestFixture]
    internal class PlantNetRequestTest
    {
        private static HttpClient CreateHttpClient()
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
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));
            // create the HttpClient
            return new HttpClient(httpMessageHandlerMock.Object);
        }

        [Test]
        public async Task PlantNetRequest_MakeRequest_CanBecalled()
        {
            //Arrange
            var loggerMock = new Mock<ILogger<PlantNetRequest>>();
            PlantNetRequest plantNetRequest = new(CreateHttpClient(), loggerMock.Object);

            var httpRequestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri("https://someuri.url"),
                Method = HttpMethod.Post,
            };

            // Act
            var response = await plantNetRequest.MakeRequest(httpRequestMessage);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}