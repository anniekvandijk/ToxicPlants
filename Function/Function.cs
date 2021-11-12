using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Function.Models;
using Function.Models.Request;
using Function.Repository;
using Function.Services;
using Function.Utilities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Function
{
    public class Function
    {
        private readonly IPlantRepository _plantRepository;
        private readonly IAnimalRepository _animalRepository;
        private readonly IToxicPlantRepository _toxicPlantsRepository;
        private readonly IPlantService _plantService;
        private ILogger logger;

        public Function(IPlantRepository plantRepository, IAnimalRepository animalRepository, IToxicPlantRepository toxicPlantsRepository, IPlantService plantService)
        {
            _plantRepository = plantRepository;
            _animalRepository = animalRepository;
            _toxicPlantsRepository = toxicPlantsRepository;
            _plantService = plantService;
        }

        //TODO: Alle error handling
        //TODO: Unittests

        [Function("plantcheck")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData request,
            FunctionContext executionContext)
        {
            logger = executionContext.GetLogger("PlantCheck");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var parsedData = await RequestParser.Parse(request.Body);
            AddAnimals(parsedData);
            await AddPlants(parsedData);
            var matchResult = MatchToxicPlantsForAnimals();

            // if content OK return OK and stuff
            var response = request.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            await response.WriteStringAsync(matchResult);

            // if not OK return not OK

            // return
            return response;
        }

        public void AddAnimals(RequestData data)
        {
            var animals = data.Parameters.Where(x => x.Name == "animal").ToList();

            if (animals.Count == 0)
            {
                throw new ApplicationException("No animal received");
            }

            foreach (var animal in animals)
            {
                if (Enum.TryParse(animal.Data, true, out Animal animalEnum))
                {
                    _animalRepository.Add(animalEnum);
                }
                else
                {
                    throw new ApplicationException("Animal not supported");
                }
            }
        }

        public async Task AddPlants(RequestData data)
        {
            var responseContent = await _plantService.GetPlantsAsync(data);

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
                _plantRepository.Add(plant);
            }
        }

        private string MatchToxicPlantsForAnimals()
        {
            foreach (var animal in _animalRepository.Get())
            {
                foreach(var plant in _plantRepository.Get())
                {
                    var ToxicPlant = _toxicPlantsRepository.GetbyAnimalAndPlantName(animal, plant);
                }
            }

            return "nothing yet";
        }


    }
}
