using Function.MiddleWare.ExceptionHandler;
using Function.Models;
using Function.Models.Request;
using Function.Repository;
using Function.Services;
using Function.Utilities;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Function.UseCases
{
    internal interface IHandleRequestData
    {
        Task<string> HandleRequest(HttpRequestData request);
    }

    internal class HandleRequestDataPlantCheck : IHandleRequestData
    {
        private readonly IPlantRepository _plantRepository;
        private readonly IAnimalRepository _animalRepository;
        private readonly IPlantService _plantService;
        private readonly IPlantAnimalRepository _plantAnimalRepository;

        public HandleRequestDataPlantCheck(IPlantRepository plantRepository, IAnimalRepository animalRepository, IPlantService plantService, IPlantAnimalRepository plantAnimalRepository)
        {
            _plantRepository = plantRepository;
            _animalRepository = animalRepository;
            _plantService = plantService;
            _plantAnimalRepository = plantAnimalRepository;
        }

        public async Task<string> HandleRequest(HttpRequestData request)
        {
            var parsedData = await RequestParser.Parse(request.Body);
            AddAnimals(parsedData);
            await AddPlants(parsedData);
            var matchResult = MatchToxicPlantsForAnimals();
            return matchResult;
        }

        private void AddAnimals(RequestData data)
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

        private async Task AddPlants(RequestData data)
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
