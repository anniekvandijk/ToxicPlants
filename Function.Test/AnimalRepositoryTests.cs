using Function.Models;
using Function.Repository;
using NUnit.Framework;

namespace Function.Tests
{
    [TestFixture]
    internal class AnimalRepositoryTests
    {
        [Test]
        public void AnimalRepository_Add_CanAddOneAnimal()
        {
            //Arrange
            AnimalRepository repo = new();

            // act
            repo.Add(Animal.Alpaca);

            // assert
            var animals = repo.Get();
            Assert.AreEqual(1, animals.Count);
            Assert.AreEqual(Animal.Alpaca, animals[0]);
        }

        [Test]
        public void AnimalRepository_Add_AddMultipleAnimalRegisterOnlyOne()
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
