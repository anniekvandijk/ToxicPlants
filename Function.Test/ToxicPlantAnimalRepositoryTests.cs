using Function.MiddleWare.ExceptionHandler;
using Function.Models;
using Function.Repository;
using NUnit.Framework;
using System.Collections.Generic;

namespace Function.Tests
{
    [TestFixture]
    internal class ToxicPlantAnimalRepositoryTests
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
                ScientificClassification = ScientificClassification.Species,
                Species = "Some strange name",
                Summary = "Some summary information",
                InformationReferences = new List<InformationReference>
                {
                    new InformationReference
                    {
                        Reference = "An reference",
                        Information = "Some info"
                    }
                }

            };

            // Act
            repo.Add(toxicPlantAnimal);

            // Assert
            var toxicPlantAnimals = repo.Get();
            Assert.AreEqual(1, toxicPlantAnimals.Count);
            Assert.AreEqual(Animal.Alpaca, toxicPlantAnimals[0].Animal);
            Assert.AreEqual(1, toxicPlantAnimals[0].HowToxic);
            Assert.AreEqual("Some strange name", toxicPlantAnimals[0].Species);
            Assert.AreEqual("An reference", toxicPlantAnimals[0].InformationReferences[0].Reference);
            Assert.AreEqual("Some info", toxicPlantAnimals[0].InformationReferences[0].Information);
            Assert.AreEqual("Some summary information", toxicPlantAnimals[0].Summary);
        }

        [Test]
        [TestCase(0)]
        [TestCase(4)]
        public void ToxicPlantAnimalRepository_Add_ToxicPlantWithInvalidToxicClassGivesProgramError(int howToxic)
        {
            //Arrange
            ToxicPlantAnimalRepository repo = new();
            ToxicPlantAnimal toxicPlantAnimal = new()
            {
                Animal = Animal.Alpaca,
                HowToxic = howToxic,
                ScientificClassification = ScientificClassification.Species,
                Species = "Some strange name",
                Summary = "Some summary information",
            };

            // Assert
            ProgramError ex = Assert.Throws<ProgramError>(() => repo.Add(toxicPlantAnimal));
            Assert.AreEqual("Toxicclass is not in range of 1-3", ex.Message);
        }

        [Test]
        public void ToxicPlantAnimalRepository_Add_ToxicPlantWithoutToxicClassGivesProgramError()
        {
            //Arrange
            ToxicPlantAnimalRepository repo = new();
            ToxicPlantAnimal toxicPlantAnimal = new()
            {
                Animal = Animal.Alpaca,
                ScientificClassification = ScientificClassification.Species,
                Species = "Some strange name",
                Summary = "Some summary information",
            };

            // Assert
            ProgramError ex = Assert.Throws<ProgramError>(() => repo.Add(toxicPlantAnimal));
            Assert.AreEqual("Toxicclass is not in range of 1-3", ex.Message);
        }

        [Test]
        public void ToxicPlantAnimalRepository_GetByAnimalName_returnsOnlyToxicPlantsForAnimal()
        {
            // Arrange
            ToxicPlantAnimalRepository repo = new();
            List<ToxicPlantAnimal> toxicPlantAnimals = new()
            {
                new()
                {
                    Genus = "Rhodondendron",
                    Animal = Animal.Alpaca,
                    HowToxic = 3,
                    ScientificClassification = ScientificClassification.Genus,
                    Summary = "Some summary information"
                },
                new()
                {
                    Species = "Hyoscyamus niger",
                    Animal = Animal.Alpaca,
                    HowToxic = 1,
                    ScientificClassification = ScientificClassification.Species,
                    Summary = "Some summary information"
                },
                new()
                {
                    Species = "Hyoscyamus niger",
                    Animal = Animal.Horse,
                    HowToxic = 2,
                    ScientificClassification = ScientificClassification.Species,
                    Summary = "Some summary information"
                }
            };
            foreach (var toxicPlantAnimal in toxicPlantAnimals)
            {
                repo.Add(toxicPlantAnimal);
            }

            var animal = Animal.Alpaca;

            // Act
            var actualResult = repo.GetByAnimalName(animal);

            // Assert
            Assert.AreEqual(2, actualResult.Count);
        }

        [Test]
        public void ToxicPlantAnimalRepository_GetbyAnimalAndPlantName_returnsToxicPlantForAnimalSpeciesIfExists()
        {
            // Arrange
            ToxicPlantAnimalRepository repo = new();
            ToxicPlantAnimal toxicPlantAnimal = new()
            {
                Species = "Prunus serotina",
                Animal = Animal.Alpaca,
                HowToxic = 2,
                ScientificClassification = ScientificClassification.Species,
                Summary = "Some summary information"
            };
            repo.Add(toxicPlantAnimal);

            var animal = Animal.Alpaca;
            Plant plant = new()
            {
                Species = "Prunus serotina",
                Genus = "Prunus",
                Family = "Rosaceae"
            };

            // Act
            var actualResult = repo.GetbyAnimalAndPlantName(animal, plant);

            // Assert
            Assert.AreEqual("Prunus serotina", actualResult[0].Species);
        }

        [Test]
        public void ToxicPlantAnimalRepository_GetbyAnimalAndPlantName_returnsToxicPlantForAnimalGenusIfExists()
        {
            // Arrange
            ToxicPlantAnimalRepository repo = new();
            ToxicPlantAnimal toxicPlantAnimal = new()
            {
                Genus = "Prunus",
                Animal = Animal.Alpaca,
                HowToxic = 2,
                ScientificClassification = ScientificClassification.Genus,
                Summary = "Some summary information"
            };
            repo.Add(toxicPlantAnimal);

            var animal = Animal.Alpaca;
            Plant plant = new()
            {
                Species = "Prunus serotina",
                Genus = "Prunus",
                Family = "Rosaceae"
            };

            // Act
            var actualResult = repo.GetbyAnimalAndPlantName(animal, plant);

            // Assert
            Assert.AreEqual("Prunus", actualResult[0].Genus);
        }

        [Test]
        public void ToxicPlantAnimalRepository_GetbyAnimalAndPlantName_returnsToxicPlantForAnimalFamilyIfExists()
        {
            // Arrange
            ToxicPlantAnimalRepository repo = new();
            ToxicPlantAnimal toxicPlantAnimal = new()
            {
                Family = "Rosaceae",
                Animal = Animal.Alpaca,
                HowToxic = 2,
                ScientificClassification = ScientificClassification.Family,
                Summary = "Some summary information"
            };
            repo.Add(toxicPlantAnimal);

            var animal = Animal.Alpaca;
            Plant plant = new()
            {
                Species = "Prunus serotina",
                Genus = "Prunus",
                Family = "Rosaceae"
            };

            // Act
            var actualResult = repo.GetbyAnimalAndPlantName(animal, plant);

            // Assert
            Assert.AreEqual("Rosaceae", actualResult[0].Family);
        }

        [Test]
        public void ToxicPlantAnimalRepository_GetbyAnimalAndPlantName_returnsNothingIfNotExists()
        {
            // Arrange
            ToxicPlantAnimalRepository repo = new();
            ToxicPlantAnimal toxicPlantAnimal = new()
            {
                Genus = "Rhodondendron",
                Animal = Animal.Alpaca,
                HowToxic = 3,
                ScientificClassification = ScientificClassification.Genus,
                Summary = "Some summary information"
            };
            repo.Add(toxicPlantAnimal);

            var animal = Animal.Alpaca;
            Plant plant = new()
            {
                Species = "not existing plant",
            };

            // Act
            var actualResult = repo.GetbyAnimalAndPlantName(animal, plant);

            // Assert
            Assert.AreEqual(0, actualResult.Count);
        }
    }
}