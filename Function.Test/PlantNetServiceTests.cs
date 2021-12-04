using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Function.Interfaces;
using Function.Models;
using Function.Models.Request;
using Function.Services;
using Moq;
using NUnit.Framework;

namespace Function.Tests
{
    [TestFixture]
    internal class PlantNetServiceTests
    {
        private static async Task<(PlantNetService service, RequestData requestData, Mock<IPlantRepository> repo)> Arrange(string fileName)
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ??
                         throw new InvalidOperationException("File not found")
                , "Data", fileName);

            var content = await File.ReadAllTextAsync(path);

            Mock<IPlantRepository> repo = new();
            Mock<IPlantRequest> request = new(); 
            RequestData requestData = new();
            HttpRequestMessage httpRequestMessage = new();
            HttpResponseMessage httpResponseMessage = new(HttpStatusCode.OK);
            httpResponseMessage.Content = new StringContent(content);

            request.Setup(x =>
                    x.MakeRequest(httpRequestMessage)).
                Returns(Task.FromResult(httpResponseMessage));

            return (new(request.Object, repo.Object), requestData, repo);
        }

        [Test]
        public async Task PlantNetService_AddPlants_CanAddPlantToPlantRepository()
        {
            // Arrange
            const string responseFile = "PlantNetResponse_OK.json";
            var (service, requestData, repo) = await Arrange(responseFile);

            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ??
                                    throw new InvalidOperationException("File not found")
                , "Data", responseFile);

            var content = await File.ReadAllTextAsync(path);

            var json = JsonSerializer.Deserialize<JsonElement>(content);
            json.TryGetProperty("results", out var results);

            var species = "species";
            var genus = "genus";
            var family = "family";
            var result = results[0];

            // Act
            service.AddPlantToRepository(species, genus, family, result);

            // Assert
            repo.Verify(m => m.Add(It.IsAny<Plant>()), Times.AtLeastOnce);
        }
    }
}