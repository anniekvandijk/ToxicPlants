using Function.Models;
using Function.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Function.Tests
{
    [TestClass]
    public class AnimalRepositoryTests
    {
        [TestMethod]
        public void AddKnownAnimal()
        {
            //Arrange
            var repo = new AnimalRepository();

            // act
            repo.Add("alpaca");

            // assert
            var animals = repo.GetAll();
            Assert.AreEqual(1, animals.Count());
        }

        [TestMethod]
        public void AddMoreOfSameAnimalNotAdded()
        {
            //Arrange
            var repo = new AnimalRepository();

            // act
            repo.Add("alpaca");
            repo.Add("alpaca");

            // assert
            var animals = repo.GetAll();
            Assert.AreEqual(1, animals.Count());
        }

        [TestMethod]
        public void AddUnknownAnimalThrows()
        {
            //Arrange
            var repo = new AnimalRepository();

            // assert
            var ex = Assert.ThrowsException<ArgumentException>(() => repo.Add("lama"));
            Assert.AreEqual("Animal not supported", ex.Message);
        }

        [TestMethod] 
        public void AddMultipleKnownAnimals()
        {
            //Arrange
            var repo = new AnimalRepository();

            var data = new RequestData()
            {
                Parameters = new List<ParameterData>
                { 
                    new ParameterData { Name = "animal", Data = "alpaca" },
                    new ParameterData { Name = "animal", Data = "paard" } 
                }
            };

            // Act
            repo.AddAll(data);

            // assert
            var animals = repo.GetAll();
            Assert.AreEqual(2, animals.Count());
        }

        [TestMethod]
        public void AddMultipleAndUnKnownAnimalsThrows()
        {
            //Arrange
            var repo = new AnimalRepository();

            var data = new RequestData()
            {
                Parameters = new List<ParameterData>
                {
                    new ParameterData { Name = "animal", Data = "alpaca" },
                    new ParameterData { Name = "animal", Data = "lama" }
                }
            };

            // assert
            var ex = Assert.ThrowsException<ArgumentException>(() => repo.AddAll(data));
            Assert.AreEqual("Animal not supported", ex.Message);
        }
    }
}
