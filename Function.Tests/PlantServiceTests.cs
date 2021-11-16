using Function.Models.Request;
using Function.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Function.Tests
{
    [TestClass]
    internal class PlantServiceTests
    {
        [TestMethod]
        [Ignore]
        public async Task GetPlantsReturnsOK()
        {
            //Arrange
            var responseFile = "PlantNetResultFile.json";

            // Instantiate the mock object
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            // Set up the SendAsync method behavior.

            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @$"TestData\PlantNetResponse\{responseFile}");
            var expectedResponseBody = File.ReadAllText(path);

            httpMessageHandlerMock
                .Protected() // <= here is the trick to set up protected!!!
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(expectedResponseBody)
                });
            // create the HttpClient
            var httpClient = new HttpClient(httpMessageHandlerMock.Object);

            // create data
            var data = new RequestData();

            //Act
            var plantNetService = new FakePlantService();
            var response = await plantNetService.GetPlantsAsync(data);
        }
    }
}
