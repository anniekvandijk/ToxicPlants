using Function.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Function.Repository
{
    public interface IPlantNetRepository
    {
        public Task<List<Plant>> GetPlants(RequestData data);
    }
}
