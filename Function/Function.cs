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
using Function.Repository;
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
        private readonly IPlantNetRepository _plantNetRepository;

        public Function(IPlantNetRepository plantNetRepository)
        {
            _plantNetRepository = plantNetRepository;
        }

        //TODO: Alle error handling
        //TODO: Unittests

        [Function("plantcheck")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData request,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("PlantCheck");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var parsedData = await ParseRequestData(request.Body);
            var animals = AnimalList(parsedData);
            var plants = await _plantNetRepository.GetPlants(parsedData);
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

        private static async Task<RequestData> ParseRequestData(Stream requestData)
        {
            MultipartFormDataParser parstData = null;
            try
            {
                parstData = await MultipartFormDataParser.ParseAsync(requestData);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            var files = parstData.Files;
            var parameters = parstData.Parameters;

            return new RequestData
            {
                Files = new List<FilePart>(files),
                Parameters = new List<ParameterPart>(parameters)
            };
        }

        private static List<Animal> AnimalList(RequestData data)
        {
            var animals = data.Parameters.Where(x => x.Name == "animal");
            var animalList = new List<Animal>();

            foreach (var animal in animals)
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

    }
}
