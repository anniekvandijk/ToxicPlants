using Function.Models;
using System.Collections.Generic;

namespace Function.Repository
{
    internal interface IPlantRepository
    {
        List<Plant> Get();
        void Add(Plant plant);
    }

    internal class PlantRepository : IPlantRepository
    {
        private readonly List<Plant> _plants;

        public PlantRepository()
        {
            _plants = new();
        }

        public List<Plant> Get() => _plants;

        public void Add(Plant plant)
        {
            _plants.Add(plant);
        }
    }
}
