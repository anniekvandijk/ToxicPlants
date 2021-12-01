using System.Threading.Tasks;
using Function.Interfaces;
using Function.Utilities;
using Microsoft.Azure.Functions.Worker.Http;

namespace Function.UseCases
{
    internal class HandleRequest : IHandleRequest
    {
        private readonly IPlantSevice _plantService;
        private readonly IAnimalSevice _animalService;

        public HandleRequest(IPlantSevice plantService, IAnimalSevice animalService)
        {
            _plantService = plantService;
            _animalService = animalService;
        }

        public async Task CollectData(HttpRequestData request)
        {

            var parsedData = await RequestParser.Parse(request.Body);

            var addPlants = _plantService.AddPlants(parsedData);
            _animalService.AddAnimals(parsedData);
            await Task.WhenAll(addPlants);
        }
    }
}
