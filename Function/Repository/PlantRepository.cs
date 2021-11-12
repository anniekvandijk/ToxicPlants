using Function.Models;
using System.Collections.Generic;
using Function.Interfaces;

namespace Function.Repository
{
    public class PlantRepository : IPlantRepository
    {
        private readonly List<Plant> _plants;

        public PlantRepository()
        {
            _plants = new();
        }

        public List<Plant> Get()
        {
            return _plants;
        }

        public void Add(Plant plant)
        {
            _plants.Add(plant);
        }
    }
}
