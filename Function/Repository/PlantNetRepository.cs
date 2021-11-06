using Function.Models;
using Function.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Function.Repository
{
    public class PlantNetRepository : IPlantNetRepository
    {
        private readonly IPlantNetService _plantNetService;
        private readonly List<Plant> plants;

        public PlantNetRepository(IPlantNetService plantNetService)
        {
            _plantNetService = plantNetService;
            plants = new();
        }

        public List<Plant> GetAll()
        {
            return plants;
        }

        public async Task AddAllAsync(RequestData data)
        {
            var responseContent = await _plantNetService.GetPlantsAsync(data);

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
