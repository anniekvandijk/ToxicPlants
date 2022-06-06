using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Function.Interfaces;
using Function.Models.Request;
using Function.Tests.Utilities;
using Function.UseCases;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Function.Tests
{
    [TestFixture]
    internal class HandleRequestTest
    {
        [Test][Ignore("can not fix testdata")]
        public async Task HandleRequest_CollectData_RequestIsParsed()
        {
            // Arrange
            Mock<IPlantSevice> plantService = new();
            Mock<IAnimalSevice> animalSevice = new();

            var requestData = Helpers.CreateDefaultRequestData();
            var requestBody = await Helpers.CreatedefaultRequestStream(requestData);

            HandleRequest handleRequest = new(plantService.Object, animalSevice.Object);

            // Act
            await handleRequest.CollectData(requestBody);
        }
    }
}