using System.Collections.Generic;
using Function.Models;

namespace Function.Interfaces
{
    public interface IPlantRepository
    {
        public List<Plant> Get();
        public void Add(Plant plant);
    }
}
