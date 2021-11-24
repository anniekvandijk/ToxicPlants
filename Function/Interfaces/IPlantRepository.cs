using Function.Models;
using System.Collections.Generic;

namespace Function.Interfaces
{
    internal interface IPlantRepository
    {
        List<Plant> Get();
        void Add(Plant plant);
    }
}
