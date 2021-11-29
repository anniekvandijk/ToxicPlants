using Function.Interfaces;
using Function.MiddleWare.ExceptionHandler;
using Function.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Function.UseCases
{
    internal class Matcher : IMatcher
    {
        private readonly ILogger<Matcher> _logger;
        private readonly IPlantRepository _plantRepository;
        private readonly IAnimalRepository _animalRepository;
        private readonly IToxicPlantAnimalRepository _toxicPlantAnimalRepository;
        private readonly IToxicPlantAnimalService _toxicPlantAnimalService;

        public Matcher(ILogger<Matcher> logger, IPlantRepository plantRepository, IAnimalRepository animalRepository, IPlantRequest plantService, IToxicPlantAnimalRepository toxicPlantAnimalRepository, IToxicPlantAnimalService toxicPlantAnimalService)
        {
            _logger = logger;
            _plantRepository = plantRepository;
            _animalRepository = animalRepository;
            _toxicPlantAnimalRepository = toxicPlantAnimalRepository;
            _toxicPlantAnimalService = toxicPlantAnimalService;
        }

        public List<ToxicPlantAnimal> MatchToxicPlantsForAnimals()
        {
            _toxicPlantAnimalService.LoadToxicPlantAnimalData();

            var plantResponseList = new List<ToxicPlantAnimal>();
            foreach (var animal in _animalRepository.Get())
            {
                foreach (var plant in _plantRepository.Get())
                {
                    var toxicPlantList = _toxicPlantAnimalRepository.GetbyAnimalAndPlantName(animal, plant);
                    switch (toxicPlantList.Count)
                    {
                        case 1:
                            {
                                var toxicPlant = toxicPlantList.First();
                                toxicPlant.PlantDetail = plant.PlantDetail;
                                plantResponseList.Add(toxicPlant);
                                break;
                            }
                        case 0:
                            var nonToxicPlant = new ToxicPlantAnimal
                            {
                                Animal = animal,
                                Species = plant.Species,
                                Genus = plant.Genus,
                                Family = plant.Family,
                                PlantDetail = plant.PlantDetail,
                                HowToxic = 0,
                                ExtraInformation = "There is no information that this plant is toxic for this animal"
                            };
                            plantResponseList.Add(nonToxicPlant);
                            break;
                        default:
                            ProgramError.CreateProgramError(HttpStatusCode.Conflict, "Multiple hits on same toxic plant.");
                            break;
                    }
                }
            }

            var nrOfToxicPlants = plantResponseList.Where(x => x.HowToxic > 0);
            _logger.LogInformation($"Found {nrOfToxicPlants.Count()} posible toxic plants hits for sent animals");
            return plantResponseList;
        }
    }
}
