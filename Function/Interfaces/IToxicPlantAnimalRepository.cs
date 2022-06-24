using Function.Models;
using System.Collections.Generic;

namespace Function.Interfaces
{
    internal interface IToxicPlantAnimalRepository
    {
        void Add(ToxicPlantAnimal plantAnimal);
        List<ToxicPlantAnimal> Get();
        ToxicPlantAnimal GetById(string id);
        List<ToxicPlantAnimal> GetByAnimalName(Animal animal);
        List<ToxicPlantAnimal> GetbyAnimalAndPlantName(Animal animal, Plant plant);
    }
}
