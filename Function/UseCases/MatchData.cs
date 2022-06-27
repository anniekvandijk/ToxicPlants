using Function.Interfaces;
using Function.MiddleWare.ExceptionHandler;
using Function.Models;
using Function.Models.Response;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Function.UseCases
{
    internal class MatchData : IMatchData
    {
        private readonly ILogger<MatchData> _logger;
        private readonly IPlantRepository _plantRepository;
        private readonly IAnimalRepository _animalRepository;
        private readonly IToxicPlantAnimalRepository _toxicPlantAnimalRepository;
        private readonly IToxicPlantAnimalService _toxicPlantAnimalService;

        public MatchData(ILogger<MatchData> logger, IPlantRepository plantRepository, IAnimalRepository animalRepository, IPlantRequest plantService, IToxicPlantAnimalRepository toxicPlantAnimalRepository, IToxicPlantAnimalService toxicPlantAnimalService)
        {
            _logger = logger;
            _plantRepository = plantRepository;
            _animalRepository = animalRepository;
            _toxicPlantAnimalRepository = toxicPlantAnimalRepository;
            _toxicPlantAnimalService = toxicPlantAnimalService;
        }

        public List<PlantResponse> MatchToxicPlantsForAnimals()
        {
            _toxicPlantAnimalService.LoadToxicPlantAnimalData();

            var plantResponseList = new List<PlantResponse>();
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

                                var infoList = new List<Models.Response.InformationReference>();
                                foreach(var information in toxicPlant.InformationReferences)
                                {
                                    var info = new Models.Response.InformationReference
                                    {
                                        Reference = information.Reference,
                                        Information = information.Information,
                                    };
                                    infoList.Add(info);
                                }

                                var plantResponse = new PlantResponse
                                {
                                    Animal = animal.ToString(),
                                    ScientificClassification = toxicPlant.ScientificClassification.ToString(),
                                    HowToxic = toxicPlant.HowToxic,
                                    Summary = toxicPlant.Summary,
                                    InformationReferences = infoList,
                                    ScientificName = plant.ScientificName,
                                    CommonNames = plant.CommonNames,
                                    Score = plant.Score,
                                    ImagesUrls = plant.ImagesUrls

                                };

                                plantResponseList.Add(plantResponse);
                                break;
                            }
                        case 0:
                            var nonToxicPlant = new PlantResponse
                            {
                                Animal = animal.ToString(),
                                HowToxic = 0,
                                ScientificName = plant.ScientificName,
                                CommonNames = plant.CommonNames,
                                Score = plant.Score,
                                ImagesUrls = plant.ImagesUrls
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
