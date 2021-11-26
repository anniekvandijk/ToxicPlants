using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Function.Models;

namespace Function.Tests.Utilities
{
    internal static class Helpers
    {
        public static void VerifyLog<T>(this Mock<ILogger<T>> mockLogger, LogLevel loglevel, string message)
        {
            mockLogger.Verify(logger => logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == loglevel),
                    It.Is<EventId>(eventId => eventId.Id == 0),
                    It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == message && @type.Name == "FormattedLogValues"),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        public static List<ToxicPlantAnimal> ToxicPlantAnimalTestData()
        {
            return new List<ToxicPlantAnimal>
            {

                new()
                {
                    PlantName = "Anthriscus sylvestris (L.) Hoffm.", 
                    Animal = Animal.Alpaca, 
                    HowToxic = 1,
                    Reference = "Alpacawereld"
                },
                new()
                {
                    PlantName = "Prunus serotina", 
                    Animal = Animal.Alpaca, 
                    HowToxic = 2,
                    Reference = "Some reference"
                },
                new()
                {
                    PlantName = "Rhodondendron", 
                    Animal = Animal.Alpaca, 
                    HowToxic = 3,
                    Reference = "Alpacawereld"
                },
                new()
                {
                    PlantName = "Hyoscyamus niger", 
                    Animal = Animal.Alpaca, 
                    HowToxic = 4,
                    Reference = "Alpacawereld"
                },
                new()
                {
                    PlantName = "Hyoscyamus niger",
                    Animal = Animal.Horse,
                    HowToxic = 5,
                    Reference = "Horseworld"
                }
            };
        }
    }
}
