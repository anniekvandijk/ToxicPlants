using Function.Models;
using System.Collections.Generic;

namespace Function.Repository
{
    public interface IToxicPlantsRepository
    {
        public List<ToxicPlant> GetByAnimalName(Animal animal);

        public ToxicPlant GetToxicPlant(Animal animal, string plantName);
    }
}
