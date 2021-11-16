using Function.Models.Request;
using System.Threading.Tasks;

namespace Function.Services
{
    internal interface IPlantService
    {
        public Task<string> GetPlantsAsync(RequestData data);
    }
}
