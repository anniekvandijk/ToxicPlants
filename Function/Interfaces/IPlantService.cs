using System.Threading.Tasks;
using Function.Models.Request;

namespace Function.Interfaces
{
    public interface IPlantService
    {
        public Task<string> GetPlantsAsync(RequestData data);
    }
}
