using Function.MiddleWare.ExceptionHandler;
using Function.Models;
using Function.Models.Request;
using Function.Repository;
using Function.Services;
using Function.Utilities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Function
{
    internal class Function
    {
        private readonly IPlantRepository _plantRepository;
        private readonly IAnimalRepository _animalRepository;
        private readonly IPlantAnimalRepository _plantAnimalRepository;
        private readonly IPlantService _plantService;
        private ILogger _logger;

        public Function(IPlantRepository plantRepository, IAnimalRepository animalRepository, IPlantAnimalRepository plantAnimalRepository, IPlantService plantService)
        {
            _plantRepository = plantRepository;
            _animalRepository = animalRepository;
            _plantAnimalRepository = plantAnimalRepository;
            _plantService = plantService;
        }


        [OpenApiOperation(operationId: "post_plants", tags: new[] { "greeting" }, Summary = "Greetings", Description = "This shows a welcome message.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter("name", Type = typeof(string), In = ParameterLocation.Query, Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Summary = "The response", Description = "This returns the response")]

        [Function("plantcheck")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/plantcheck")] HttpRequestData request,
            FunctionContext executionContext)
        {
            _logger = executionContext.GetLogger("PlantCheck");
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            _logger.LogInformation(_plantAnimalRepository.Get().Count.ToString());

            var parsedData = await RequestParser.Parse(request.Body);
            AddAnimals(parsedData);
            await AddPlants(parsedData);
            var matchResult = MatchToxicPlantsForAnimals();

            // if content OK return OK and stuff
            var response = request.CreateResponse(HttpStatusCode.OK);
            //response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            const string fakeContent =
@"{
    ""Animal"": ""Alpaca"",
    ""ScientificName"": ""Test"",
}";
            await response.WriteStringAsync(fakeContent);

            // if not OK return not OK

            // return
            return response;
        }

        public void AddAnimals(RequestData data)
        {
            var animals = data.Parameters.Where(x => x.Name == "animal").ToList();

            if (animals.Count == 0)
            {
                throw new RequestDataException("No animal received");
            }

            foreach (var animal in animals)
            {
                if (Enum.TryParse(animal.Data, true, out Animal animalEnum))
                {
                    _animalRepository.Add(animalEnum);
                }
                else
                {
                    throw new RequestDataException("Animal not supported");
                }
            }
        }

        public async Task AddPlants(RequestData data)
        {
            var responseContent = await _plantService.GetPlantsAsync(data);

            var json = JsonConvert.DeserializeObject(responseContent).ToString();
            var jsonObject = JObject.Parse(json);
            var results = (JArray)jsonObject["results"];

            foreach (var result in results)
            {
                var plant = new Plant
                {
                    ScientificName = (string)result["species"]["scientificName"],
                    Score = (double)result["score"]
                };
                _plantRepository.Add(plant);
            }
        }

        private string MatchToxicPlantsForAnimals()
        {
            foreach (var animal in _animalRepository.Get())
            {
                foreach (var plant in _plantRepository.Get())
                {
                    var ToxicPlant = _plantAnimalRepository.GetbyAnimalAndPlantName(animal, plant);
                }
            }

            return "nothing yet";
        }


    }
}
