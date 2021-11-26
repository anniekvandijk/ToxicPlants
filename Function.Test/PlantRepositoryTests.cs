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
        public void PlantRepository_Add_CanAddOnePlant()
        {
            //Arrange
            var loggerMock = new Mock<ILogger<PlantRepository>>();
            PlantRepository repo = new(loggerMock.Object);

            Plant plant = new()
            {
                ScientificName = "Some strange name",
            };

            // act
            repo.Add(plant);

            // assert
            var plants = repo.Get();
            Assert.AreEqual(1, plants.Count);
            Assert.AreEqual("Some strange name", plants[0].ScientificName);
        }

        [Test]
        public void PlantRepository_Add_AddingPlantWithSameNameIsLoggedAndNotAddedAgain()
        {
            //Arrange
            var loggerMock = new Mock<ILogger<PlantRepository>>();
            PlantRepository repo = new(loggerMock.Object);

            Plant plant1 = new()
            {
                ScientificName = "Some strange name",
            };

            Plant plant2 = new()
            {
                ScientificName = "Some strange name",
            };

            // act

            repo.Add(plant1);
            repo.Add(plant2);

            // assert
            var plants = repo.Get();
            Assert.AreEqual(1, plants.Count);
            loggerMock.VerifyLog(LogLevel.Critical, "Plant with same Scenttific name was not added to the Plant repository. Plantname = Some strange name");


        }
    }
}