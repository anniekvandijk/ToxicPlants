using Function.Models.Request;
using System.Threading.Tasks;

namespace Function.Interfaces
{
    internal interface IPlantService
    {
        public Task<string> GetPlantsAsync(RequestData data);
    }
}
