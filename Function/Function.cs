using Function.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Function.Utilities;

namespace Function
{
    internal class Function
    {
        private readonly IPlantSevice _plantService;
        private readonly IAnimalSevice _animalService;
        private readonly IHandleResponse _handleResponse;
        private readonly IMatcher _matcher;
        private ILogger _logger;

        public Function(IAnimalSevice animalService, IPlantSevice plantService, IHandleResponse handleResponse, IMatcher matcher)
        {
            _plantService = plantService;
            _animalService = animalService;
            _handleResponse = handleResponse;
            _matcher = matcher;
        }

        [Function("plantcheck")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/plantcheck")] HttpRequestData request,
            FunctionContext executionContext)
        {
            _logger = executionContext.GetLogger("PlantCheck");
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            // If something goes wrong, all is handled by the ExceptionHandlerMiddleware

            var parsedData = await RequestParser.Parse(request.Body);
            
            var addPlants = _plantService.AddPlants(parsedData);
            _animalService.AddAnimals(parsedData);
            await Task.WhenAll(addPlants);

            var result = _matcher.MatchToxicPlantsForAnimals();
            
            return await _handleResponse.SetResponse(request, result);
        }
    }
}
