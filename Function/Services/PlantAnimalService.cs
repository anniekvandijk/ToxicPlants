using Function.Models;
using Function.Repository;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Function.Services
{
    internal interface IPlantAnimalService
    {
        void LoadPlantAnimalData();
    }

    internal class PlantAnimalService : IPlantAnimalService
    {
        private readonly IPlantAnimalRepository _plantAnimalRepository;
        private readonly ILogger<PlantAnimalService> _logger;

        public PlantAnimalService(IPlantAnimalRepository plantAnimalRepository, ILogger<PlantAnimalService> logger)
        {
            _plantAnimalRepository = plantAnimalRepository;
            _logger = logger;
            LoadPlantAnimalData();
        }

        public void LoadPlantAnimalData()
        {
            var plantAnimalList = new List<PlantAnimal>
                {

                    new()
                    {
                        PlantName = "Adonis aestivalis", Animal = Animal.Alpaca, HowToxic = "very",
                        Reference = "Alpacawereld"
                    },
                    new()
                    {
                        PlantName = "Prunus serotina", Animal = Animal.Alpaca, HowToxic = "very",
                        Reference = "Alpacawereld"
                    },
                    new()
                    {
                        PlantName = "Rhodondendron", Animal = Animal.Alpaca, HowToxic = "very",
                        Reference = "Alpacawereld"
                    },
                    new()
                    {
                        PlantName = "Hyoscyamus niger", Animal = Animal.Alpaca, HowToxic = "very",
                        Reference = "Alpacawereld"
                    }
                };

            foreach (var plantAnimal in plantAnimalList)
            {
                _plantAnimalRepository.Add(plantAnimal);
            }

            _logger.LogDebug("Data toxic plants loaded");
        }
    }
}
