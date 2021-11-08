using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Function.Models;
using Function.Repository;
using Function.Services;
using Function.Utilities;
using HttpMultipartParser;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Function
{
    public class Function
    {
        private IPlantRepository _plantRepository;
        private IAnimalRepository _animalRepository;
        private IToxicPlantRepository _toxicPlantsRepository;

        public Function(IPlantRepository plantRepository, IAnimalRepository animalRepository, IToxicPlantRepository toxicPlantsRepository)
        {
            _plantRepository = plantRepository;
            _animalRepository = animalRepository;
            _toxicPlantsRepository = toxicPlantsRepository;
        }

        //TODO: Alle error handling
        //TODO: Unittests

        [Function("plantcheck")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData request,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("PlantCheck");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var parsedData = await RequestParser.Parse(request.Body);
            _animalRepository.AddAll(parsedData);
            await _plantRepository.AddAllAsync(parsedData);
            var matchResult = MatchToxicPlantsForAnimals();

            // if content OK return OK and stuff
            var response = request.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            await response.WriteStringAsync(matchResult);

            // if not OK return not OK

            // return
            return response;
        }

        private string MatchToxicPlantsForAnimals()
        {
            foreach (var animal in _animalRepository.GetAll())
            {
                foreach(var plant in _plantRepository.GetAll())
                {
                    var ToxicPlant = _toxicPlantsRepository.GetbyAnimalAndPlantName(animal, plant);
                }
            }

            return "nothing yet";
        }


    }
}
