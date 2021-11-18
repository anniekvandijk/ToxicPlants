using Function.Interfaces;
using Function.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Function.Services
{
    internal class ToxicPlantAnimalService : IToxicPlantAnimalService
    {
        private readonly IToxicPlantAnimalRepository _toxicPlantAnimalRepository;
        private readonly ILogger<ToxicPlantAnimalService> _logger;

        public ToxicPlantAnimalService(IToxicPlantAnimalRepository toxicPlantAnimalRepository, ILogger<ToxicPlantAnimalService> logger)
        {
            _toxicPlantAnimalRepository = toxicPlantAnimalRepository;
            _logger = logger;
            LoadPlantAnimalData();
        }

        public void LoadPlantAnimalData()
        {
            var plantAnimalList = new List<ToxicPlantAnimal>
                {

                    new()
                    {
                        PlantName = "Anthriscus sylvestris (L.) Hoffm.", Animal = Animal.Alpaca, HowToxic = "very",
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
                _toxicPlantAnimalRepository.Add(plantAnimal);
            }

            _logger.LogDebug("Data toxic plants loaded");
        }
    }
}
