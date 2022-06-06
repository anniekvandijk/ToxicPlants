using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Function.Interfaces;
using Function.MiddleWare.ExceptionHandler;
using Function.Models;
using Function.Services;
using Function.Tests.Utilities;
using Moq;
using NUnit.Framework;

namespace Function.Tests
{
    [TestFixture]
    internal class PlantNetServiceTests
    {
        [SetUp]
        public void SetUpFixture()
        {
            Environment.SetEnvironmentVariable("PLANTNET_URL", "https://www.myfakeplantnet.url/all?api-key=secretapikey");
        }

        [Test]
        public async Task PlantNetService_AddPlants_CanAddPlantToPlantRepository()
        {

            // Arrange
            const string responseFile = "PlantNetResponse_OK.json";

            var requestData = Helpers.CreateDefaultRequestData();

            var (service, repo, request) = await ArrangePlantNetServiceMock(responseFile, HttpStatusCode.OK);

            // Act
            await service.AddPlants(requestData);

            // Assert
            repo.Verify(m => m.Add(It.IsAny<Plant>()), Times.AtLeastOnce);
        }

        [Test]
        public async Task PlantNetService_AddPlants_EmptyResultsCreatesProgramError()
        {

            // Arrange
            const string responseFile = "PlantNetResponse_EmptyResults.json";

            var requestData = Helpers.CreateDefaultRequestData();

            var (service, repo, request) = await ArrangePlantNetServiceMock(responseFile, HttpStatusCode.OK);

            // Assert
            ProgramError ex = Assert.ThrowsAsync<ProgramError>(async() => await service.AddPlants(requestData));
            Assert.AreEqual("No results received from plantrequest", ex.Message);
        }

        [Test]
        public async Task PlantNetService_AddPlants_NoResultsCreatesProgramError()
        {

            // Arrange
            const string responseFile = "PlantNetResponse_NoResults.json";

            var requestData = Helpers.CreateDefaultRequestData();

            var (service, repo, request) = await ArrangePlantNetServiceMock(responseFile, HttpStatusCode.OK);

            // Assert
            ProgramError ex = Assert.ThrowsAsync<ProgramError>(async () => await service.AddPlants(requestData));
            Assert.AreEqual("Error receiving plants from plantrequest", ex.Message);
        }

        [Test]
        public async Task PlantNetService_AddPlants_NotDeserializebleResultCreatesProgramError()
        {

            // Arrange
            const string responseFile = "PlantNetResponse_NoJson.json";

            var requestData = Helpers.CreateDefaultRequestData();

            var (service, repo, request) = await ArrangePlantNetServiceMock(responseFile, HttpStatusCode.OK);

            // Assert
            ProgramError ex = Assert.ThrowsAsync<ProgramError>(async () => await service.AddPlants(requestData));
            Assert.AreEqual("No results received from plantrequest", ex.Message);
        }

        [Test]
        public async Task PlantNetService_AddPlants_NotSuccessResponseWithUnexpectedContentCreatesProgramError()
        {

            // Arrange
            const string responseFile = "PlantNetResponse_NOK_WrongContent.json";

            var requestData = Helpers.CreateDefaultRequestData();

            var (service, repo, request) = await ArrangePlantNetServiceMock(responseFile, HttpStatusCode.InternalServerError);

            // Assert
            ProgramError ex = Assert.ThrowsAsync<ProgramError>(async () => await service.AddPlants(requestData));
            Assert.AreEqual("Something went wrong with the request", ex.Message);
        }

        [Test]
        public async Task PlantNetService_AddPlants_NotSuccessResponseWithExpectedContentCreatesProgramError()
        {

            // Arrange
            const string responseFile = "PlantNetResponse_NOK_ExpectedContent.json";

            var requestData = Helpers.CreateDefaultRequestData();

            var (service, repo, request) = await ArrangePlantNetServiceMock(responseFile, HttpStatusCode.InternalServerError);

            // Assert
            ProgramError ex = Assert.ThrowsAsync<ProgramError>(async () => await service.AddPlants(requestData));
            Assert.AreEqual("\"Organs\" is required", ex.Message);
        }

        [Test]
        public async Task PlantNetService_AddPlants_NoSpeciesInResultsCreatesProgramError()
        {

            // Arrange
            const string responseFile = "PlantNetResponse_NoSpecies.json";

            var requestData = Helpers.CreateDefaultRequestData();

            var (service, repo, request) = await ArrangePlantNetServiceMock(responseFile, HttpStatusCode.OK);

            // Assert
            ProgramError ex = Assert.ThrowsAsync<ProgramError>(async () => await service.AddPlants(requestData));
            Assert.AreEqual("Error receiving plantdetails from plantrequest", ex.Message);
        }

        [Test]
        public async Task PlantNetService_AddPlants_NoGenusInResultsCreatesProgramError()
        {

            // Arrange
            const string responseFile = "PlantNetResponse_NoGenus.json";

            var requestData = Helpers.CreateDefaultRequestData();

            var (service, repo, request) = await ArrangePlantNetServiceMock(responseFile, HttpStatusCode.OK);

            // Assert
            ProgramError ex = Assert.ThrowsAsync<ProgramError>(async () => await service.AddPlants(requestData));
            Assert.AreEqual("Error receiving plantdetails from plantrequest", ex.Message);
        }

        [Test]
        public async Task PlantNetService_AddPlants_NoFamilyInResultsCreatesProgramError()
        {

            // Arrange
            const string responseFile = "PlantNetResponse_NoFamily.json";

            var requestData = Helpers.CreateDefaultRequestData();

            var (service, repo, request) = await ArrangePlantNetServiceMock(responseFile, HttpStatusCode.OK);

            // Assert
            ProgramError ex = Assert.ThrowsAsync<ProgramError>(async () => await service.AddPlants(requestData));
            Assert.AreEqual("Error receiving plantdetails from plantrequest", ex.Message);
        }

        private static async Task<(PlantNetService service, Mock<IPlantRepository> repo, Mock<IPlantRequest> request)> ArrangePlantNetServiceMock(string fileName, HttpStatusCode httpStatusCode)
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ??
                                    throw new InvalidOperationException("File not found")
                , "Data", fileName);

            var content = await File.ReadAllTextAsync(path);

            Mock<IPlantRepository> repo = new();
            Mock<IPlantRequest> request = new();
            HttpResponseMessage httpResponseMessage = new(httpStatusCode);
            httpResponseMessage.Content = new StringContent(content);

            request.Setup(x =>
                    x.MakeRequest(It.IsAny<HttpRequestMessage>())).
                Returns(Task.FromResult(httpResponseMessage));

            return (new(request.Object, repo.Object), repo, request);
        }
    }
}
