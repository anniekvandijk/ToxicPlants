using System.Threading.Tasks;
using Function.Models.Request;

namespace Function.Interfaces
{
    internal interface IPlantSevice
    {
        Task AddPlants(RequestData data);
    }
}