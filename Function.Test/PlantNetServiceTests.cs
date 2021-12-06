using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Function.Interfaces;
using Function.Models;
using Function.Models.Request;
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

        private static async Task<(PlantNetService service, Mock<IPlantRepository> repo)> ArrangePlantNetServiceMock(string fileName, HttpStatusCode httpStatusCode)
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

            return (new(request.Object, repo.Object), repo);
        }

        [Test]
        public async Task PlantNetService_AddPlants_CanAddPlantToPlantRepository()
        {

            // Arrange
            const string responseFile = "PlantNetResponse_OK.json";
            const string imageName = "plant.jpg";

            var fileDataList = new List<FileData>();

            FileData fileData = new()
            {
                Data = Helpers.CreateFileStream(imageName),
                ContentType = "image/jpeg",
                Name = "images",
                FileName = imageName

            };
            fileDataList.Add(fileData);

            var parameterDataList = new List<ParameterData>()
            {
                new()
                {
                    Name = "organs",
                    Data = "flower"
                }

            };

            var requestData = new RequestData
            {
                Files = fileDataList,
                Parameters = parameterDataList
            };

            var (service, repo) = await ArrangePlantNetServiceMock(responseFile, HttpStatusCode.OK);

            // Act
            await service.AddPlants(requestData);

            // Assert
            repo.Verify(m => m.Add(It.IsAny<Plant>()), Times.AtLeastOnce);
        }
    }
}