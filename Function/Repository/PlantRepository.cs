using Function.Models;
using System.Collections.Generic;
using Function.Interfaces;

namespace Function.Repository
{
    public class PlantRepository : IPlantRepository
    {
        private readonly List<Plant> plants;

        public PlantRepository()
        {
            plants = new();
        }

        public List<Plant> Get()
        {
            return plants;
        }

        public void Add(Plant plant)
        {
            plants.Add(plant);
        }
    }
}
