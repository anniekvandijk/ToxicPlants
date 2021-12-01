using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Function.Interfaces;
using Function.MiddleWare.ExceptionHandler;
using Function.Models;
using Function.Models.Request;

namespace Function.Services
{
    internal class PlantNetService : IPlantSevice
    {
        private readonly IPlantRequest _plantRequest;
        private readonly IPlantRepository _plantRepository;

        public PlantNetService(IPlantRequest plantRequest, IPlantRepository plantRepository)
        {
            _plantRequest = plantRequest;
            _plantRepository = plantRepository;
        }

        public async Task AddPlants(RequestData data)
        {
            var results = await GetPlantRequestResults(data);

            if (results.GetArrayLength() > 0)
            {
                foreach (var result in results.EnumerateArray())
                {
                    GetResultDetails(result, out var species, out var genus, out var family);

                    var plant = new Plant
                    {
                        Species = species,
                        Genus = genus,
                        Family = family,
                        PlantDetail = result
                    };
                    _plantRepository.Add(plant);
                }
            }
            else
                ProgramError.CreateProgramError(HttpStatusCode.InternalServerError, "Nor results received from plantreqest");
        }

        private async Task<JsonElement> GetPlantRequestResults(RequestData data)
        {
            JsonElement results = default;
            try
            {
                var responseContent = await _plantRequest.GetPlantsAsync(data);
                var json = JsonSerializer.Deserialize<JsonElement>(responseContent);
                json.TryGetProperty("results", out results);
            }
            catch (Exception ex)
            {
                ProgramError.CreateProgramError(HttpStatusCode.InternalServerError, "Error receiving plants from plantrequest",
                    ex);
            }

            return results;
        }

        private static void GetResultDetails(JsonElement result, out string species, out string genus, out string family)
        {
            species = result.GetProperty("species")
                .GetProperty("scientificNameWithoutAuthor")
                .GetString();

            genus = result.GetProperty("species")
                .GetProperty("genus")
                .GetProperty("scientificNameWithoutAuthor")
                .GetString();

            family = result.GetProperty("species")
                .GetProperty("family")
                .GetProperty("scientificNameWithoutAuthor")
                .GetString();

            if (species == null || genus == null || family == null)
                ProgramError.CreateProgramError(HttpStatusCode.InternalServerError,
                    "Error receiving plantdetails from plantrequest");
        }
    }
}
