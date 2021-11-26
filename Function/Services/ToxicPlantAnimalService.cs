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
        }

        public void LoadToxicPlantAnimalData()
        {
            if (_toxicPlantAnimalRepository.Get().Count == 0)
            {
                var plantAnimalList = new List<ToxicPlantAnimal>
                {

                    new()
                    {
                        PlantName = "Anthriscus sylvestris (L.) Hoffm.", Animal = Animal.Alpaca, HowToxic = 1,
                        Reference = "Alpacawereld"
                    },
                    new()
                    {
                        PlantName = "Prunus serotina", Animal = Animal.Alpaca, HowToxic = 3,
                        Reference = "Alpacawereld"
                    },
                    new()
                    {
                        PlantName = "Rhodondendron", Animal = Animal.Alpaca, HowToxic = 2,
                        Reference = "Alpacawereld"
                    },
                    new()
                    {
                        PlantName = "Hyoscyamus niger", Animal = Animal.Alpaca, HowToxic = 5,
                        Reference = "Alpacawereld"
                    }
                };

                foreach (var plantAnimal in plantAnimalList)
                {
                    _toxicPlantAnimalRepository.Add(plantAnimal);
                }
                _logger.LogInformation("Toxic plant data loaded");
            }
            else
            {
                _logger.LogInformation("Toxic plant data already loaded");
            }
        }
    }
}
