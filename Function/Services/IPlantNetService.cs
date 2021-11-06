using Function.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Function.Services
{
    public interface IPlantNetService
    {
        public Task<List<Plant>> GetPlantsAsync(RequestData data);
    }
}
