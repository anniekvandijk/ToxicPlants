using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Function.Interfaces;
using Function.MiddleWare.ExceptionHandler;
using Function.Models;
using Function.Models.Request;
using Function.Utilities;

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
                    AddPlantToRepository(species, genus, family, result);
                }
            }
            else
                ProgramError.CreateProgramError(HttpStatusCode.InternalServerError, "No results received from plantrequest");
        }

        private void AddPlantToRepository(string species, string genus, string family, JsonElement result)
        {
            var plant = new Plant
            {
                Species = species,
                Genus = genus,
                Family = family,
                PlantDetail = result
            };
            _plantRepository.Add(plant);
        }

        private async Task<JsonElement> GetPlantRequestResults(RequestData data)
        {
            var content = CreateMultipartFormDataContentAsync(data);
            var language = GetLanguage(data);

            JsonElement results = default;
            var responseContent = await MakePlantNetRequest(content, language);
            try
            {
                var json = JsonSerializer.Deserialize<JsonElement>(responseContent);
                results = json.GetProperty("results");
            }
            catch (Exception ex)
            {
                ProgramError.CreateProgramError(HttpStatusCode.InternalServerError, "Error receiving plants from plantrequest",
                    ex);
            }

            return results;
        }

        private static MultipartFormDataContent CreateMultipartFormDataContentAsync(RequestData data)
        {
            var images = data.Files.Where(x => x.Name.ToLower() == "images");
            var organs = data.Parameters.Where(x => x.Name.ToLower() == "organs");

            var multiPartContent = new MultipartFormDataContent();

            foreach (var image in images)
            {
                var fileContent = new StreamContent(image.Data);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(image.ContentType);
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = image.Name,
                    FileName = image.FileName
                };
                multiPartContent.Add(fileContent);
            }

            foreach (var organ in organs)
            {
                var keyValue = new KeyValuePair<string, string>(organ.Name, organ.Data);
                multiPartContent.Add(new StringContent(keyValue.Value), keyValue.Key);
            }

            return multiPartContent;
        }

        private static string GetLanguage(RequestData data)
        {
            var language = data.Parameters.SingleOrDefault(x => x.Name.ToLower() == "language");
            if (language == null || language.Data.TrimToNull() == null) return "en";
            return language.Data.ToLower();
        }

        private async Task<string> MakePlantNetRequest(MultipartFormDataContent content, string language)
        {
            var url = $"{Environment.GetEnvironmentVariable("PLANTNET_URL")}&lang={language}";

            var plantRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Post,
                Content = content
            };

            var response = await _plantRequest.MakeRequest(plantRequest);
            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return result;
            }
            else
            {
                var json = JsonSerializer.Deserialize<JsonElement>(result);

                HttpStatusCode statusCode = (HttpStatusCode)0;
                string message = null;
                try
                {
                    statusCode = (HttpStatusCode)json.GetProperty("statusCode").GetInt16();
                    message = json.GetProperty("message").GetString();
                }
                catch (Exception ex)
                {
                    ProgramError.CreateProgramError(HttpStatusCode.InternalServerError, "Something went wrong with the request", ex);
                }
                ProgramError.CreateProgramError(statusCode, message, 1); // quit
                return null;
            }
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
