using System;
using System.IO;
using System.Reflection;
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

            request.Setup(x =>
                    x.GetPlantsAsync(requestData)).
                Returns(Task.FromResult(content));

            return (new(request.Object, repo.Object), requestData, repo);
        }

        [Test]
        public async Task PlantNetService_AddPlants_CanAddPlantToPlantRepository()
        {
            // Arrange
            const string responseFile = "PlantNetResponse_OK.json";
            var (service, requestData, repo) = await Arrange(responseFile);

            // Act
            await service.AddPlants(requestData);

            // Assert
            repo.Verify(m => m.Add(It.IsAny<Plant>()), Times.AtLeastOnce);
        }
    }
}