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
    internal class FakePlantRequestTest
    {
        [Test]
        public async Task FakePlantRequest_MakeRequest_CanBecalledAndReturnsContent()
        {
            //Arrange
            Environment.SetEnvironmentVariable("FAKE_PLANTREQUEST_FILENAME", "PlantNetToxic.json");
            
            var loggerMock = new Mock<ILogger<FakePlantRequest>>();
            FakePlantRequest fakePlantRequest = new(loggerMock.Object);

            var httpRequestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri("https://someuri.url"),
                Method = HttpMethod.Post,
            };

            // Act
            var response = await fakePlantRequest.MakeRequest(httpRequestMessage);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotEmpty(response.Content.ReadAsStringAsync().Result);
        }
    }
}