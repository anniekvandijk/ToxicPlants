using Function.Models;
using Function.Models.Request;
using Function.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Function.Repository
{
    public class PlantRepository : IPlantRepository
    {
        private readonly IPlantService _plantService;
        private readonly List<Plant> plants;

        public PlantRepository(IPlantService plantService)
        {
            _plantService = plantService;
            plants = new();
        }

        public List<Plant> GetAll()
        {
            return plants;
        }

        public async Task AddAllAsync(RequestData data)
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
                plants.Add(plant);
            }
        }
    }
}
