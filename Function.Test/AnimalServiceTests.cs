using Function.Interfaces;
using Function.MiddleWare.ExceptionHandler;
using Function.Models;
using Function.Models.Request;
using Function.Services;
using Moq;
using NUnit.Framework;

namespace Function.Tests
{
    [TestFixture]
    internal class AnimalServiceTests
    {
        [Test]
        public void AnimalService_AddAnimals_CanAddAnimalToAnimalRepository()
        {
            // Arrange
            Mock<IAnimalRepository> repo = new();

            RequestData requestData = new()
            {
                Parameters = new()
                {
                    new ()
                    {
                        Name = "animal",
                        Data = "Alpaca"
                    }
                }
            };

            AnimalSevice service = new(repo.Object);

            // Act
            service.AddAnimals(requestData);

            // Assert
            repo.Verify(m => m.Add(Animal.Alpaca), Times.Once());
        }

        [Test]
        public void AnimalService_AddAnimals_CanAddAnimalToAnimalRepositoryWithStrangeCasing()
        {
            // Arrange
            Mock<IAnimalRepository> repo = new();

            RequestData requestData = new RequestData
            {
                Parameters = new()
                {
                    new()
                    {
                        Name = "anImAl",
                        Data = "AlpaCa"
                    }
                }
            };

            AnimalSevice service = new(repo.Object);

            // Act
            service.AddAnimals(requestData);

            // Assert
            repo.Verify(m => m.Add(Animal.Alpaca), Times.Once());
        }

        [Test]
        public void AnimalService_AddAnimals_NoAnimalWillCreatePorgamError()
        {
            // Arrange
            Mock<IAnimalRepository> repo = new();

            RequestData requestData = new RequestData
            {
                Parameters = new()
                {
                }
            };

            AnimalSevice service = new(repo.Object);

            // Assert
            ProgramError ex = Assert.Throws<ProgramError>(() => service.AddAnimals(requestData));
            Assert.AreEqual("No animal parameter in request", ex.Message);

            repo.Verify(m => m.Add(Animal.Alpaca), Times.Never);
        }

        [Test]
        public void AnimalService_AddAnimals_UnknownAnimalWillCreatePorgamError()
        {
            // Arrange
            Mock<IAnimalRepository> repo = new();

            RequestData requestData = new RequestData
            {
                Parameters = new()
                {
                    new()
                    {
                        Name = "animal",
                        Data = "Lama"
                    }
                }
            };

            AnimalSevice service = new(repo.Object);

            // Assert
            ProgramError ex = Assert.Throws<ProgramError>(() => service.AddAnimals(requestData));
            Assert.AreEqual("No animal or unsupported animal in request", ex.Message);

            repo.Verify(m => m.Add(Animal.Alpaca), Times.Never);
        }

        [Test]
        public void AnimalService_AddAnimals_EmptyAnimalWillCreatePorgamError()
        {
            // Arrange
            Mock<IAnimalRepository> repo = new();

            RequestData requestData = new RequestData
            {
                Parameters = new()
                {
                    new()
                    {
                        Name = "animal",
                        Data = ""
                    }
                }
            };

            AnimalSevice service = new(repo.Object);

            // Assert
            ProgramError ex = Assert.Throws<ProgramError>(() => service.AddAnimals(requestData));
            Assert.AreEqual("No animal or unsupported animal in request", ex.Message);

            repo.Verify(m => m.Add(Animal.Alpaca), Times.Never);
        }

        [Test]
        public void AnimalService_AddAnimals_NullAnimalWillCreatePorgamError()
        {
            // Arrange
            Mock<IAnimalRepository> repo = new();

            RequestData requestData = new RequestData
            {
                Parameters = new()
                {
                    new()
                    {
                        Name = "animal",
                        Data = null
                    }
                }
            };

            AnimalSevice service = new(repo.Object);

            // Assert
            ProgramError ex = Assert.Throws<ProgramError>(() => service.AddAnimals(requestData));
            Assert.AreEqual("No animal or unsupported animal in request", ex.Message);

            repo.Verify(m => m.Add(Animal.Alpaca), Times.Never);
        }
    }
}