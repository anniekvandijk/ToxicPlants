using Function.Models;
using System.Threading.Tasks;

namespace Function.Services
{
    public interface IPlantService
    {
        public Task<string> GetPlantsAsync(RequestData data);
    }
}
