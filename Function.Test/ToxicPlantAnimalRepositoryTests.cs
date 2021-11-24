using Function.Models;
using Function.Repository;
using Function.Tests.Utilities;
using NUnit.Framework;

namespace Function.Tests
{
    [TestFixture]
    public class ToxicPlantAnimalRepositoryTests
    {
        [Test]
        public void ToxicPlantAnimalRepository_Add_CanAddOneToxicPlantAnimal()
        {
            //Arrange
            ToxicPlantAnimalRepository repo = new();
            ToxicPlantAnimal toxicPlantAnimal = new()
            {
                Animal = Animal.Alpaca,
                HowToxic = "No clue",
                PlantName = "Some strange name",
                Reference = "An reference"
            };

            // Act
            repo.Add(toxicPlantAnimal);

            // Assert
            var toxicPlantAnimals = repo.Get();
            Assert.AreEqual(1, toxicPlantAnimals.Count);
            Assert.AreEqual(Animal.Alpaca, toxicPlantAnimals[0].Animal);
            Assert.AreEqual("No clue", toxicPlantAnimals[0].HowToxic);
            Assert.AreEqual("Some strange name", toxicPlantAnimals[0].PlantName);
            Assert.AreEqual("An reference", toxicPlantAnimals[0].Reference);
        }

        [Test]
        public void ToxicPlantAnimalRepository_GetByAnimalName_returnsOnlyToxicPlantsForAnimal()
        {
            // Arrange
            ToxicPlantAnimalRepository repo = new();
            foreach (var toxicPlantAnimal in Helpers.ToxicPlantAnimalTestData())
            {
                repo.Add(toxicPlantAnimal);
            }

            var animal = Animal.Alpaca;

            // Act
            var actualResult = repo.GetByAnimalName(animal);

            // Assert
            Assert.AreEqual(4, actualResult.Count);
        }

        [Test]
        public void ToxicPlantAnimalRepository_GetbyAnimalAndPlantName_returnsToxicPlantForAnimalIfExists()
        {
            // Arrange
            ToxicPlantAnimalRepository repo = new();
            foreach (var toxicPlantAnimal in Helpers.ToxicPlantAnimalTestData())
            {
                repo.Add(toxicPlantAnimal);
            }

            var animal = Animal.Alpaca;
            Plant plant = new()
            {
                ScientificName = "Prunus serotina",
                Score = 0.1,
                CommonNames = new[] { "name 1", "Name 2" }
            };

            // Act
            var actualResult = repo.GetbyAnimalAndPlantName(animal, plant);

            // Assert
            Assert.AreEqual("Prunus serotina", actualResult[0].PlantName);
        }

        [Test]
        public void ToxicPlantAnimalRepository_GetbyAnimalAndPlantName_returnsIfNotExists()
        {
            // Arrange
            ToxicPlantAnimalRepository repo = new();
            foreach (var toxicPlantAnimal in Helpers.ToxicPlantAnimalTestData())
            {
                repo.Add(toxicPlantAnimal);
            }

            var animal = Animal.Alpaca;
            Plant plant = new()
            {
                ScientificName = "not existing plant",
                Score = 0.1,
                CommonNames = new[] { "name 1", "Name 2" }
            };

            // Act
            var actualResult = repo.GetbyAnimalAndPlantName(animal, plant);

            // Assert
            Assert.AreEqual(0, actualResult.Count);
        }
    }
}