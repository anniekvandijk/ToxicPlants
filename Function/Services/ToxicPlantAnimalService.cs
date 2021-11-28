using System;
using Function.Interfaces;
using Function.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Function.Models.Request;

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

        public async Task LoadToxicPlantAnimalDataFromFile()
        {
            _logger.LogInformation("Loader toxicplants called");

            var path =
                Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidOperationException("File not found")
                    , @"Data\ToxicPlants.csv");
            var file = await File.ReadAllTextAsync(path);
        }

        public void LoadToxicPlantAnimalData()
        {
            if (_toxicPlantAnimalRepository.Get().Count == 0)
            {
                var plantAnimalList = new List<ToxicPlantAnimal>
                {

                    new()
                    {
                        Species = "Anthriscus sylvestris", 
                        Animal = Animal.Alpaca, 
                        HowToxic = 1,
                        ScientificClassification = ScientificClassification.Species,
                        Reference = "Alpacawereld"
                    },
                    new()
                    {
                        Species = "Prunus serotina", 
                        Animal = Animal.Alpaca, 
                        HowToxic = 3,
                        ScientificClassification = ScientificClassification.Species,
                        Reference = "Alpacawereld"
                    },
                    new()
                    {
                        Species = "Rhodondendron", 
                        Animal = Animal.Alpaca, 
                        HowToxic = 3,
                        ScientificClassification = ScientificClassification.Genus,
                        Reference = "Alpacawereld"
                    },
                    new()
                    {
                        Species = "Hyoscyamus niger", 
                        Animal = Animal.Alpaca, 
                        HowToxic = 3,
                        ScientificClassification = ScientificClassification.Species,
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
