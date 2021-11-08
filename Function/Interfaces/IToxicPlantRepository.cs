using Function.Models;
using System.Collections.Generic;

namespace Function.Repository
{
    public interface IToxicPlantRepository
    {
        public List<ToxicPlant> GetByAnimalName(Animal animal);

        public ToxicPlant GetbyAnimalAndPlantName(Animal animal, Plant plant);
    }
}
