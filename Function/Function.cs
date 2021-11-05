using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Function.Models;
using Function.Services;
using HttpMultipartParser;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Function
{
    public class Function
    {
        private readonly HttpClient _httpClient;
        private readonly IEnvironmentVariableService _environmentVariableService;
        
        public Function(HttpClient httpClient, IEnvironmentVariableService environmentVariableService)
        {
            _httpClient = httpClient;
            _environmentVariableService = environmentVariableService;
        }

        //TODO: Alle error handling
        //TODO: Unittests

        [Function("plantcheck")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData request,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("PlantCheck");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var parsedData = await ParseRequestBody(request.Body);
            var animals = AnimalList(parsedData);
            var plants = await GetPlants(parsedData);
            var matchResult = MatchToxicPlantsForAnimals(plants, animals);

            // if content OK return OK and stuff
            var response = request.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            await response.WriteStringAsync(matchResult);

            // if not OK return not OK

            // return
            return response;
        }

        private string MatchToxicPlantsForAnimals(List<Plant> plants, List<Animal> animals)
        {
            return "nothing yet";
        }

        private static async Task<MultiPartFormData> ParseRequestBody(Stream requestBody)
        {
            MultipartFormDataParser parsedFormBody = null;
            try
            {
                parsedFormBody = await MultipartFormDataParser.ParseAsync(requestBody);
            } 
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            var files = parsedFormBody.Files;
            var parameters = parsedFormBody.Parameters;

            return new MultiPartFormData
            {
                Files = new List<FilePart>(files),
                Parameters = new List<ParameterPart>(parameters)
            };
        }

        private static MultipartFormDataContent CreateMultipartFormDataContentAsync(MultiPartFormData data)
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

        private static List<Animal> AnimalList(MultiPartFormData data)
        {
            var animals = data.Parameters.Where(x => x.Name == "animal");
            var animalList = new List<Animal>();
            
            foreach(var animal in animals)
            {
                if (Enum.TryParse(animal.Data, true, out Animal animalEnum))
                {
                    animalList.Add(animalEnum);
                }
                else
                {
                    // error?
                }
            }

            return animalList;
        }

        private async Task<List<Plant>> GetPlants(MultiPartFormData data)
        {
            // Make call to PlantNet and get response
            var content = CreateMultipartFormDataContentAsync(data);
            var response = await MakePlantNetRequest(content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return CreatePlantList(responseContent);
            }
            else
            {
                // errorhandling
                return null;
            }
           
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

        private static List<Plant> CreatePlantList(string content)
        {
            var list = new List<Plant>();
            var json = JsonConvert.DeserializeObject(content).ToString();
            JObject jsonObject = JObject.Parse(json);
            JArray results = (JArray)jsonObject["results"];
            foreach (var result in results)
            {
                var plant = new Plant
                {
                    Name = (string)result["species"]["scientificName"],
                    Score = (double)result["score"]
                };
                list.Add(plant);
            }

            return list;
        }
    }
}
