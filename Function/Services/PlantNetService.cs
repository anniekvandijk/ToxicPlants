using Function.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Function.Services
{
    public class PlantNetService : IPlantNetService
    {
        private readonly HttpClient _httpClient;
        private readonly IEnvironmentVariableService _environmentVariableService;

        public PlantNetService(HttpClient httpClient, IEnvironmentVariableService environmentVariableService)
        {
            _httpClient = httpClient;
            _environmentVariableService = environmentVariableService;
        }

        public async Task<List<Plant>> GetPlantsAsync(RequestData data)
        {
            // Make call to PlantNet and get response
            var content = CreateMultipartFormDataContentAsync(data);
            var response = await MakePlantNetRequest(content);

            var plantList = new List<Plant>();

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject(responseContent).ToString();
                JObject jsonObject = JObject.Parse(json);
                JArray results = (JArray)jsonObject["results"];
                foreach (var result in results)
                {
                    var plant = new Plant
                    {
                        Name = (string)result["species"]["scientificName"],
                        Score = (double)result["score"]
                    };
                    plantList.Add(plant);
                }
            }
            else
            {
                // TODO: errorhandling
            }
            return plantList;
        }

        private static MultipartFormDataContent CreateMultipartFormDataContentAsync(RequestData data)
        {
            var images = data.Files.Where(x => x.Name == "images");
            var organs = data.Parameters.Where(x => x.Name == "organs");

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

        private async Task<HttpResponseMessage> MakePlantNetRequest(MultipartFormDataContent content)
        {
            var plantRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(_environmentVariableService.GetPlantNetUrl()),
                Method = HttpMethod.Post,
                Content = content
            };

            return await _httpClient.SendAsync(plantRequest);
        }
    }
}

