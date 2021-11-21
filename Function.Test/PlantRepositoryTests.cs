using System;
using Function.MiddleWare.ExceptionHandler;
using Function.Models;
using Function.Repository;
using Function.Tests.Utilities;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Function.Tests
{
    [TestFixture]
    internal class PlantRepositoryTests
    {
        [Test]
        public void PlantRepository_AddPlant_CanAddOnePlant()
        {
            //Arrange
            var loggerMock = new Mock<ILogger<PlantRepository>>();
            PlantRepository repo = new(loggerMock.Object);

            var plant = new Plant
            {
                ScientificName = "Some strange name",
                Score = 0.1,
                CommonNames = new[] { "name 1", "Name 2" }
            };

            // act

            repo.Add(plant);

            // assert
            var plants = repo.Get();
            Assert.AreEqual(1, plants.Count);
        }

        [Test]
        public void PlantRepository_AddPlant_AddingPlantWithSameNameIsLoggedAndNotAddedAgain()
        {
            //Arrange
            var loggerMock = new Mock<ILogger<PlantRepository>>();
            PlantRepository repo = new(loggerMock.Object);

            var plant1 = new Plant
            {
                ScientificName = "Some strange name",
                Score = 0.1,
                CommonNames = new[] { "name 1", "Name 2" }
            };

            var plant2 = new Plant
            {
                ScientificName = "Some strange name",
                Score = 0.1,
                CommonNames = new[] { "name 1", "Name 2" }
            };

            // act

            repo.Add(plant1);
            repo.Add(plant2);

            // assert
            var plants = repo.Get();
            Assert.AreEqual(1, plants.Count);
            VerifyHelpers.VerifyLogger(loggerMock, LogLevel.Critical, "Plant with same Scenttific name was not added to the Plant repository. Plantname = Some strange name");


        }


    }
}