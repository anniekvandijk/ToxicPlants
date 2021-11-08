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
            var repo = new AnimalRepository();

            var animal = Animal.Alpaca;

            // act
            repo.Add(animal);

            // assert
            var animals = repo.Get();
            Assert.AreEqual(1, animals.Count);
        }

    }
}
