using Function.Models;
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
using Function.Interfaces;

namespace Function.Tests
{
    [TestClass]
    public class PlantServiceTests
    {
        [TestMethod][Ignore]
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
                .ReturnsAsync(new HttpResponseMessage { 
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(expectedResponseBody)
                });
            // create the HttpClient
            var httpClient = new HttpClient(httpMessageHandlerMock.Object);

            var environmentVariableService = new Mock<IEnvironmentVariableService>();
            environmentVariableService
                .Setup(x => x.GetPlantUrl())
                .Returns("https://somewebpage.com");

            // create data
            var data = new RequestData();

            //Act
            var plantNetService = new PlantService(httpClient, environmentVariableService.Object);
            var response = await plantNetService.GetPlantsAsync(data);
        }
    }
}
