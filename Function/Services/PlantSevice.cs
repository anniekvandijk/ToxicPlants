using System.Text.Json;
using System.Threading.Tasks;
using Function.Interfaces;
using Function.Models;
using Function.Models.Request;
using Microsoft.Extensions.Logging;

namespace Function.Services
{
    internal class PlantSevice : IPlantSevice
    {
        private readonly IPlantRequest _plantRequest;
        private readonly IPlantRepository _plantRepository;
        private readonly ILogger<PlantSevice> _logger;

        public PlantSevice(IPlantRequest plantRequest, IPlantRepository plantRepository, ILogger<PlantSevice> logger)
        {
            _plantRequest = plantRequest;
            _plantRepository = plantRepository;
            _logger = logger;
        }

        public async Task AddPlants(RequestData data)
        {
            var responseContent = await _plantRequest.GetPlantsAsync(data);

            var json = JsonSerializer.Deserialize<JsonElement>(responseContent);

            json.TryGetProperty("results", out var results);

            foreach (var result in results.EnumerateArray())
            {
                result.TryGetProperty("species", out var species);
                species.TryGetProperty("scientificNameWithoutAuthor", out var speciesScientificNameWithoutAuthor);

                species.TryGetProperty("genus", out var genus);
                genus.TryGetProperty("scientificNameWithoutAuthor", out var genusScientificNameWithoutAuthor);

                species.TryGetProperty("family", out var family);
                family.TryGetProperty("scientificNameWithoutAuthor", out var familyScientificNameWithoutAuthor);


                var plant = new Plant
                {
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
