using System.Threading.Tasks;
using Function.Models.Request;

namespace Function.Services
{
    public interface IPlantService
    {
        public Task<string> GetPlantsAsync(RequestData data);
    }
}
