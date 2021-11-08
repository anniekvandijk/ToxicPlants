using Function.Models;
using System.Collections.Generic;

namespace Function.Repository
{
    public interface IPlantRepository
    {
        public List<Plant> Get();
        public void Add(Plant plant);
    }
}
