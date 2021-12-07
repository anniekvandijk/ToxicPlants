using System.Threading.Tasks;
using Function.Interfaces;
using Function.Utilities;
using System.IO;

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

        public async Task CollectData(Stream requestBody)
        {

            var parsedData = await RequestParser.Parse(requestBody);

            var addPlants = _plantService.AddPlants(parsedData);
            _animalService.AddAnimals(parsedData);
            await Task.WhenAll(addPlants);
        }
    }
}
