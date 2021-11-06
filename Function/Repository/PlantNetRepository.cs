using Function.Models;
using Function.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Function.Repository
{
    public class PlantNetRepository : IPlantNetRepository
    {
        private readonly IPlantNetService _plantNetService;
        private static readonly List<Plant> plants = new();

        public PlantNetRepository(IPlantNetService plantNetService)
        {
            _plantNetService = plantNetService;
        }

        public List<Plant> GetAll()
        {
            return plants;
        }

        public async Task AddAllAsync(RequestData data)
        {
            var plantList = await _plantNetService.GetPlantsAsync(data);
            foreach (var plant in plantList)
            {
                plants.Add(plant);
            }
        }
    }
}
