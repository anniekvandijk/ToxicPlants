using Function.MiddleWare.ExceptionHandler;
using Function.Models;
using Function.Repository;
using Function.Tests.Utilities;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Text.Json;

namespace Function.Tests
{
    [TestFixture]
    internal class PlantRepositoryTests
    {
        public static JsonElement CreateJsonElement()
        {
            var content = @"{ ""content"": ""content""}";
            return JsonSerializer.Deserialize<JsonElement>(content);
        }

        [Test]
        public void PlantRepository_Add_CanAddOnePlant()
        {
            //Arrange
            var loggerMock = new Mock<ILogger<PlantRepository>>();
            PlantRepository repo = new(loggerMock.Object);

            Plant plant = new()
            {
                Species = "Some strange name",
                Genus = "Some",
                Family = "Family",
                PlantDetail = CreateJsonElement()
            };

            // act
            repo.Add(plant);

            // assert
            var plants = repo.Get();
            Assert.AreEqual(1, plants.Count);
            Assert.AreEqual("Some strange name", plants[0].Species);
        }

        [Test]
        public void PlantRepository_Add_AddingPlantWithSameSpeciesIsLoggedAndNotAddedAgain()
        {
            //Arrange
            var loggerMock = new Mock<ILogger<PlantRepository>>();
            PlantRepository repo = new(loggerMock.Object);

            Plant plant1 = new()
            {
                Species = "Some strange name",
                Genus = "Some",
                Family = "Family",
                PlantDetail = CreateJsonElement()
            };

            Plant plant2 = new()
            {
                Species = "Some strange name",
                Genus = "Some",
                Family = "Family",
                PlantDetail = CreateJsonElement()
            };

            // act

            repo.Add(plant1);
            repo.Add(plant2);

            // assert
            var plants = repo.Get();
            Assert.AreEqual(1, plants.Count);
            loggerMock.VerifyLog(LogLevel.Critical, "Plant with same Species was not added to the Plant repository. Plantname = Some strange name");
        }


        [Test]
        public void PlantRepository_Add_PlantWithoutSpeciesGivesProgramError()
        {
            //Arrange
            var loggerMock = new Mock<ILogger<PlantRepository>>();
            PlantRepository repo = new(loggerMock.Object);

            Plant plant = new()
            {
                Genus = "Some",
                Family = "Family",
                PlantDetail = CreateJsonElement()
            };

            // Assert
            ProgramError ex = Assert.Throws<ProgramError>(() => repo.Add(plant));
            Assert.AreEqual("Species can not be empty", ex.Message);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("    ")]
        public void PlantRepository_Add_PlantWithInvalidSpeciesGivesProgramError(string species)
        {
            //Arrange
            var loggerMock = new Mock<ILogger<PlantRepository>>();
            PlantRepository repo = new(loggerMock.Object);

            Plant plant = new()
            {
                Species = species,
                Genus = "Some",
                Family = "Family",
                PlantDetail = CreateJsonElement()
            };

            // Assert
            ProgramError ex = Assert.Throws<ProgramError>(() => repo.Add(plant));
            Assert.AreEqual("Species can not be empty", ex.Message);
        }

        [Test]
        public void PlantRepository_Add_PlantWithoutGenusGivesProgramError()
        {
            //Arrange
            var loggerMock = new Mock<ILogger<PlantRepository>>();
            PlantRepository repo = new(loggerMock.Object);

            Plant plant = new()
            {
                Species = "Some strange name",
                Family = "Family",
                PlantDetail = CreateJsonElement()
            };

            // Assert
            ProgramError ex = Assert.Throws<ProgramError>(() => repo.Add(plant));
            Assert.AreEqual("Genus can not be empty", ex.Message);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("    ")]
        public void PlantRepository_Add_PlantWithInvalidGenusGivesProgramError(string genus)
        {
            //Arrange
            var loggerMock = new Mock<ILogger<PlantRepository>>();
            PlantRepository repo = new(loggerMock.Object);

            Plant plant = new()
            {
                Species = "Some strange name",
                Genus = genus,
                Family = "Family",
                PlantDetail = CreateJsonElement()
            };

            // Assert
            ProgramError ex = Assert.Throws<ProgramError>(() => repo.Add(plant));
            Assert.AreEqual("Genus can not be empty", ex.Message);
        }

        [Test]
        public void PlantRepository_Add_PlantWithoutFamilyGivesProgramError()
        {
            //Arrange
            var loggerMock = new Mock<ILogger<PlantRepository>>();
            PlantRepository repo = new(loggerMock.Object);

            Plant plant = new()
            {
                Species = "Some strange name",
                Genus = "Some",
                PlantDetail = CreateJsonElement()
            };

            // Assert
            ProgramError ex = Assert.Throws<ProgramError>(() => repo.Add(plant));
            Assert.AreEqual("Family can not be empty", ex.Message);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("    ")]
        public void PlantRepository_Add_PlantWithInvalidFamilyGivesProgramError(string family)
        {
            //Arrange
            var loggerMock = new Mock<ILogger<PlantRepository>>();
            PlantRepository repo = new(loggerMock.Object);

            Plant plant = new()
            {
                Species = "Some strange name",
                Genus = "Some",
                Family = family,
                PlantDetail = CreateJsonElement()
            };

            // Assert
            ProgramError ex = Assert.Throws<ProgramError>(() => repo.Add(plant));
            Assert.AreEqual("Family can not be empty", ex.Message);
        }
    }
}