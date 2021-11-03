using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Function.Tests
{
    [TestClass]
    public class FunctionTests
    {
        [TestMethod]
        public async Task GetPlantsReturnsOK()
        {
            //Arrange
            // Instantiate the mock object
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            // Set up the SendAsync method behavior.
            httpMessageHandlerMock
                .Protected() // <= here is the trick to set up protected!!!
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage { 
                    StatusCode = HttpStatusCode.OK
                });
            // create the HttpClient
            var httpClient = new HttpClient(httpMessageHandlerMock.Object);

            //Act
            var function = new Function(httpClient);
            var response = await function.GetPlants("http://localhost");

            //Assert
            Assert.AreEqual(200, (int)response.StatusCode);
        }
    }
}
