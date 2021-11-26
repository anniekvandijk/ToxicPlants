﻿using Function.Interfaces;
using Function.MiddleWare.ExceptionHandler;
using Function.Models;
using Function.Models.Request;
using Function.Utilities;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Function.Models.Response;

namespace Function.UseCases
{
    internal class HandleRequest : IHandleRequest
    {
        private readonly ILogger<HandleRequest> _logger;
        private readonly IPlantRepository _plantRepository;
        private readonly IAnimalRepository _animalRepository;
        private readonly IPlantService _plantService;
        private readonly IToxicPlantAnimalRepository _toxicPlantAnimalRepository;

        public HandleRequest(ILogger<HandleRequest> logger, IPlantRepository plantRepository, IAnimalRepository animalRepository, IPlantService plantService, IToxicPlantAnimalRepository toxicPlantAnimalRepository)
        {
            _logger = logger;
            _plantRepository = plantRepository;
            _animalRepository = animalRepository;
            _plantService = plantService;
            _toxicPlantAnimalRepository = toxicPlantAnimalRepository;
        }

        public async Task<string> Handle(HttpRequestData request)
        {
            var parsedData = await RequestParser.Parse(request.Body);
            var addPlants = AddPlants(parsedData);
            AddAnimals(parsedData);
            await Task.WhenAll(addPlants);
            var result = MatchToxicPlantsForAnimals();
            var json = JsonSerializer.Serialize(result);
            return json;
        }

        private void AddAnimals(RequestData data)
        {
            var animals = data.Parameters.Where(x => x.Name == "animal").ToList();

            if (animals.Count == 0)
            {
                ProgramError.CreateProgramError(HttpStatusCode.BadRequest, "No animal found");
            }

            foreach (var animal in animals)
            {
                if (Enum.TryParse(animal.Data, true, out Animal animalEnum))
                {
                    _animalRepository.Add(animalEnum);
                }
                else
                {
                    ProgramError.CreateProgramError(HttpStatusCode.BadRequest, "Animal not supported");
                }
            }
            _logger.LogInformation($"Added {animals.Count} animals to AnimalRepository.");
        }

        private async Task AddPlants(RequestData data)
        {
            var responseContent = await _plantService.GetPlantsAsync(data);

            var json = JsonSerializer.Deserialize<JsonElement>(responseContent);
            
            json.TryGetProperty("results", out var results);

            foreach (var result in results.EnumerateArray())
            {
                result.TryGetProperty("species", out var species);
                species.TryGetProperty("scientificName", out var scientificName);

                var plant = new Plant
                {
                    ScientificName = scientificName.GetString(),
                    PlantDetail = result
                };
                _plantRepository.Add(plant);
            }
            _logger.LogInformation($"Added {results.GetArrayLength()} plants to PlantRepository.");
        }

        private List<PlantResponse> MatchToxicPlantsForAnimals()
        {
            var plantResponseList = new List<PlantResponse>();
            foreach (var animal in _animalRepository.Get())
            {
                foreach (var plant in _plantRepository.Get())
                {
                    var plantResponse = new PlantResponse
                    {
                        Animal = animal,
                        PlantName = plant.ScientificName,
                        PlantDetail = plant.PlantDetail
                    };

                    var toxicPlantList = _toxicPlantAnimalRepository.GetbyAnimalAndPlantName(animal, plant);
                    if (toxicPlantList.Count == 1)
                    {
                        var toxicPlant = toxicPlantList.First();

                        plantResponse.HowToxic = toxicPlant.HowToxic;
                        plantResponse.Reference = toxicPlant.Reference;
                        plantResponseList.Add(plantResponse);
                    } else if (toxicPlantList.Count == 0)
                    {
                        plantResponse.HowToxic = 0;
                        plantResponseList.Add(plantResponse);
                    } 
                    else if (toxicPlantList.Count > 1)
                    {
                        ProgramError.CreateProgramError(HttpStatusCode.Conflict, "Multiple hits on same toxic plant.");
                    }
                }
            }

            var nrOfToxicPlants = plantResponseList.Where(x => x.HowToxic > 0);
            _logger.LogInformation($"Found {nrOfToxicPlants.Count()} posible toxic plants hits for sent animals");
            return plantResponseList;
        }


    }
}
