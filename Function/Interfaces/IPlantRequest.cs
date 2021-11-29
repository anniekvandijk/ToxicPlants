using Function.Models.Request;
using System.Threading.Tasks;

namespace Function.Interfaces
{
    internal interface IPlantRequest
    {
        public Task<string> GetPlantsAsync(RequestData data);
    }
}
