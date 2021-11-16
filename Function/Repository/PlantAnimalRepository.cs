using Function.Models;
using System.Collections.Generic;
using System.Linq;

namespace Function.Repository
{
    internal interface IPlantAnimalRepository
    {
        void Add(PlantAnimal plantAnimal);
        List<PlantAnimal> Get();
        List<PlantAnimal> GetByAnimalName(Animal animal);
        List<PlantAnimal> GetByPlantName(Plant plant);
        PlantAnimal GetbyAnimalAndPlantName(Animal animal, Plant plant);
    }

    internal class PlantAnimalRepository : IPlantAnimalRepository
    {
        private readonly List<PlantAnimal> _plantAnimals;

        public PlantAnimalRepository()
        {
            _plantAnimals = new();
        }

        public void Add(PlantAnimal plantAnimal)
        {
            _plantAnimals.Add(plantAnimal);
        }

        public List<PlantAnimal> Get() => _plantAnimals;

        public List<PlantAnimal> GetByAnimalName(Animal animal) =>
            _plantAnimals
                .Where(x => x.Animal == animal).ToList();

        public List<PlantAnimal> GetByPlantName(Plant plant) =>
            _plantAnimals
                .Where(x => x.PlantName == plant.ScientificName).ToList();

        public PlantAnimal GetbyAnimalAndPlantName(Animal animal, Plant plant) =>
            _plantAnimals.SingleOrDefault(x => x.Animal == animal && x.PlantName == plant.ScientificName);
    }
}
