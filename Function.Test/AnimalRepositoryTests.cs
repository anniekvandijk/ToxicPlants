using Function.Models;
using Function.Repository;
using NUnit.Framework;

namespace Function.Tests
{
    [TestFixture]
    internal class AnimalRepositoryTests
    {
        [Test]
        public void AnimalRepository_AddAnimal_CanAddOneAnimal()
        {
            //Arrange
            AnimalRepository repo = new();

            // act
            repo.Add(Animal.Alpaca);

            // assert
            var animals = repo.Get();
            Assert.AreEqual(1, animals.Count);
        }

        [Test]
        public void AnimalRepository_AddAnimal_AddMultipleAnimalRegisterOnlyOne()
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
