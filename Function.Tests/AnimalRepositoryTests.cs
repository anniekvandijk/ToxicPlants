using Function.Models;
using Function.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Function.Tests
{
    [TestClass]
    public class AnimalRepositoryTests
    {
        [TestMethod]
        public void AddAnimal()
        {
            //Arrange
            AnimalRepository repo = new();

            // act
            repo.Add(Animal.Alpaca);

            // assert
            var animals = repo.Get();
            Assert.AreEqual(1, animals.Count);
        }

        [TestMethod]
        public void AddSameAnimalTwice_onlyOneAdded()
        {
            // arrange
            AnimalRepository repo = new();

            // act
            repo.Add(Animal.Alpaca);
            repo.Add(Animal.Alpaca);

            // assert
            var animals = repo.Get();
            Assert.AreEqual(1, animals.Count);
        }

    }
}
