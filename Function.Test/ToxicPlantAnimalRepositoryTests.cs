using Function.MiddleWare.ExceptionHandler;
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
                HowToxic = 1,
                PlantName = "Some strange name",
                Reference = "An reference"
            };

            // Act
            repo.Add(toxicPlantAnimal);

            // Assert
            var toxicPlantAnimals = repo.Get();
            Assert.AreEqual(1, toxicPlantAnimals.Count);
            Assert.AreEqual(Animal.Alpaca, toxicPlantAnimals[0].Animal);
            Assert.AreEqual(1, toxicPlantAnimals[0].HowToxic);
            Assert.AreEqual("Some strange name", toxicPlantAnimals[0].PlantName);
            Assert.AreEqual("An reference", toxicPlantAnimals[0].Reference);
        }

        [Test]
        public void ToxicPlantAnimalRepository_Add_ToxicPlantWithToxicClassLowerThan1GivesProgramError()
        {
            //Arrange
            ToxicPlantAnimalRepository repo = new();
            ToxicPlantAnimal toxicPlantAnimal = new()
            {
                Animal = Animal.Alpaca,
                HowToxic = 0,
                PlantName = "Some strange name",
                Reference = "An reference"
            };

            // Assert
            ProgramError ex = Assert.Throws<ProgramError>(() => repo.Add(toxicPlantAnimal));
            Assert.AreEqual("Toxicclass is not in range of 1-5", ex.Message);
        }


        [Test]
        public void ToxicPlantAnimalRepository_Add_ToxicPlantWithToxicClassHigherThan5GivesProgramError()
        {
            //Arrange
            ToxicPlantAnimalRepository repo = new();
            ToxicPlantAnimal toxicPlantAnimal = new()
            {
                Animal = Animal.Alpaca,
                HowToxic = 6,
                PlantName = "Some strange name",
                Reference = "An reference"
            };

            // Assert
            ProgramError ex = Assert.Throws<ProgramError>(() => repo.Add(toxicPlantAnimal));
            Assert.AreEqual("Toxicclass is not in range of 1-5", ex.Message);
        }

        [Test]
        public void ToxicPlantAnimalRepository_Add_ToxicPlantWithoutToxicClassHigherThan5GivesProgramError()
        {
            //Arrange
            ToxicPlantAnimalRepository repo = new();
            ToxicPlantAnimal toxicPlantAnimal = new()
            {
                Animal = Animal.Alpaca,
                PlantName = "Some strange name",
                Reference = "An reference"
            };

            // Assert
            ProgramError ex = Assert.Throws<ProgramError>(() => repo.Add(toxicPlantAnimal));
            Assert.AreEqual("Toxicclass is not in range of 1-5", ex.Message);
        }

        [Test]
        public void ToxicPlantAnimalRepository_Add_ToxicPlantWithNoAnimalGivesError()
        {
            //Arrange
            ToxicPlantAnimalRepository repo = new();
            ToxicPlantAnimal toxicPlantAnimal = new()
            {
                HowToxic = 3,
                PlantName = "Some strange name",
                Reference = "An reference"
            };

            // Assert
            ProgramError ex = Assert.Throws<ProgramError>(() => repo.Add(toxicPlantAnimal));
            Assert.AreEqual("Animal can not be empty", ex.Message);
        }

        [Test]
        public void ToxicPlantAnimalRepository_Add_ToxicPlantWithoutPlantNameGivesProgramError()
        {
            //Arrange
            ToxicPlantAnimalRepository repo = new();
            ToxicPlantAnimal toxicPlantAnimal = new()
            {
                Animal = Animal.Alpaca,
                HowToxic = 3,
                Reference = "An reference"
            };

            // Assert
            ProgramError ex = Assert.Throws<ProgramError>(() => repo.Add(toxicPlantAnimal));
            Assert.AreEqual("PlantName can not be empty", ex.Message);
        }

        [Test]
        public void ToxicPlantAnimalRepository_Add_ToxicPlantWithNullPlantNameGivesProgramError()
        {
            //Arrange
            ToxicPlantAnimalRepository repo = new();
            ToxicPlantAnimal toxicPlantAnimal = new()
            {
                Animal = Animal.Alpaca,
                HowToxic = 3,
                PlantName = null,
                Reference = "An reference"
            };

            // Assert
            ProgramError ex = Assert.Throws<ProgramError>(() => repo.Add(toxicPlantAnimal));
            Assert.AreEqual("PlantName can not be empty", ex.Message);
        }

        [Test]
        public void ToxicPlantAnimalRepository_Add_ToxicPlantWithEmptyPlantNameGivesProgramError()
        {
            //Arrange
            ToxicPlantAnimalRepository repo = new();
            ToxicPlantAnimal toxicPlantAnimal = new()
            {
                Animal = Animal.Alpaca,
                HowToxic = 3,
                PlantName = "",
                Reference = "An reference"
            };

            // Assert
            ProgramError ex = Assert.Throws<ProgramError>(() => repo.Add(toxicPlantAnimal));
            Assert.AreEqual("PlantName can not be empty", ex.Message);
        }

        [Test]
        public void ToxicPlantAnimalRepository_Add_ToxicPlantWithOnlySpaceInPlantNameGivesProgramError()
        {
            //Arrange
            ToxicPlantAnimalRepository repo = new();
            ToxicPlantAnimal toxicPlantAnimal = new()
            {
                Animal = Animal.Alpaca,
                HowToxic = 3,
                PlantName = "   ",
                Reference = "An reference"
            };

            // Assert
            ProgramError ex = Assert.Throws<ProgramError>(() => repo.Add(toxicPlantAnimal));
            Assert.AreEqual("PlantName can not be empty", ex.Message);
        }

        [Test]
        public void ToxicPlantAnimalRepository_Add_ToxicPlantWithoutReferenceGivesProgramError()
        {
            //Arrange
            ToxicPlantAnimalRepository repo = new();
            ToxicPlantAnimal toxicPlantAnimal = new()
            {
                Animal = Animal.Alpaca,
                HowToxic = 3,
                PlantName = "Some strange name",
            };

            // Assert
            ProgramError ex = Assert.Throws<ProgramError>(() => repo.Add(toxicPlantAnimal));
            Assert.AreEqual("Reference can not be empty", ex.Message);
        }

        [Test]
        public void ToxicPlantAnimalRepository_Add_ToxicPlantWithEmptyReferenceGivesProgramError()
        {
            //Arrange
            ToxicPlantAnimalRepository repo = new();
            ToxicPlantAnimal toxicPlantAnimal = new()
            {
                Animal = Animal.Alpaca,
                HowToxic = 3,
                PlantName = "Some strange name",
                Reference = ""
            };

            // Assert
            ProgramError ex = Assert.Throws<ProgramError>(() => repo.Add(toxicPlantAnimal));
            Assert.AreEqual("Reference can not be empty", ex.Message);
        }

        [Test]
        public void ToxicPlantAnimalRepository_Add_ToxicPlantWittNullReferenceGivesProgramError()
        {
            //Arrange
            ToxicPlantAnimalRepository repo = new();
            ToxicPlantAnimal toxicPlantAnimal = new()
            {
                Animal = Animal.Alpaca,
                HowToxic = 3,
                PlantName = "Some strange name",
                Reference = null
            };

            // Assert
            ProgramError ex = Assert.Throws<ProgramError>(() => repo.Add(toxicPlantAnimal));
            Assert.AreEqual("Reference can not be empty", ex.Message);
        }

        [Test]
        public void ToxicPlantAnimalRepository_Add_ToxicPlantWithOnlySpaceInReferenceGivesProgramError()
        {
            //Arrange
            ToxicPlantAnimalRepository repo = new();
            ToxicPlantAnimal toxicPlantAnimal = new()
            {
                Animal = Animal.Alpaca,
                HowToxic = 3,
                PlantName = "Some strange name",
                Reference = "   "
            };

            // Assert
            ProgramError ex = Assert.Throws<ProgramError>(() => repo.Add(toxicPlantAnimal));
            Assert.AreEqual("Reference can not be empty", ex.Message);
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
            };

            // Act
            var actualResult = repo.GetbyAnimalAndPlantName(animal, plant);

            // Assert
            Assert.AreEqual(0, actualResult.Count);
        }
    }
}