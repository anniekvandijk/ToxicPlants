using Function.Interfaces;
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

        public HandleRequest(ILogger<HandleRequest> logger, IPlantRepository plantRepository, IAnimalRepository animalRepository, IPlantService plantService)
        {
            _logger = logger;
            _plantRepository = plantRepository;
            _animalRepository = animalRepository;
            _plantService = plantService;
        }

        public async Task Handle(HttpRequestData request)
        {
            var parsedData = await RequestParser.Parse(request.Body);
            var addPlants = AddPlants(parsedData);
            AddAnimals(parsedData);
            await Task.WhenAll(addPlants);
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
                species.TryGetProperty("scientificNameWithoutAuthor", out var speciesScientificNameWithoutAuthor);

                species.TryGetProperty("genus", out var genus);
                genus.TryGetProperty("scientificNameWithoutAuthor", out var genusScientificNameWithoutAuthor);

                species.TryGetProperty("family", out var family);
                family.TryGetProperty("scientificNameWithoutAuthor", out var familyScientificNameWithoutAuthor);


                var plant = new Plant
                {
                    ScientificName = scientificName.GetString(),
                    Species = speciesScientificNameWithoutAuthor.GetString(),
                    Genus = genusScientificNameWithoutAuthor.GetString(),
                    Family = familyScientificNameWithoutAuthor.GetString(),
                    PlantDetail = result
                };
                _plantRepository.Add(plant);
            }
            _logger.LogInformation($"Added {results.GetArrayLength()} plants to PlantRepository.");
        }
    }
}
