using Function.Interfaces;
using Function.Models;
using System.Collections.Generic;
using System.Linq;

namespace Function.Repository
{
    internal class ToxicPlantAnimalRepository : IToxicPlantAnimalRepository
    {
        private static List<ToxicPlantAnimal> _toxicPlantAnimals;

        public ToxicPlantAnimalRepository()
        {
            _toxicPlantAnimals ??= new();
        }

        public void Add(ToxicPlantAnimal plantAnimal)
        {
            _toxicPlantAnimals.Add(plantAnimal);
        }

        public List<ToxicPlantAnimal> Get() => _toxicPlantAnimals;

        public List<ToxicPlantAnimal> GetByAnimalName(Animal animal) =>
            _toxicPlantAnimals
                .Where(x => x.Animal == animal).ToList();

        public List<ToxicPlantAnimal> GetbyAnimalAndPlantName(Animal animal, Plant plant) =>
            _toxicPlantAnimals.Where(x => x.Animal == animal && x.PlantName == plant.ScientificName).ToList();
    }
}
