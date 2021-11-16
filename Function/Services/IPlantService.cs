using System.Threading.Tasks;
using Function.Models.Request;

namespace Function.Services
{
    internal interface IPlantService
    {
        public Task<string> GetPlantsAsync(RequestData data);
    }
}
