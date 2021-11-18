using Function.Models;
using System.Collections.Generic;

namespace Function.Interfaces
{
    internal interface IToxicPlantAnimalRepository
    {
        void Add(ToxicPlantAnimal plantAnimal);
        List<ToxicPlantAnimal> Get();
        List<ToxicPlantAnimal> GetByAnimalName(Animal animal);
        List<ToxicPlantAnimal> GetByPlantName(Plant plant);
        ToxicPlantAnimal GetbyAnimalAndPlantName(Animal animal, Plant plant);
    }
}
