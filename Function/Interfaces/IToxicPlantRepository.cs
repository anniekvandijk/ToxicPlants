using System.Collections.Generic;
using Function.Models;

namespace Function.Interfaces
{
    public interface IToxicPlantRepository
    {
        public List<ToxicPlant> GetByAnimalName(Animal animal);

        public ToxicPlant GetbyAnimalAndPlantName(Animal animal, Plant plant);
    }
}
