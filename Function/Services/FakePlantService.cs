using Function.Models;
using Function.Models.Request;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Function.Services
{
    public class FakePlantService : IPlantService
    {
        public async Task<string> GetPlantsAsync(RequestData data)
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Utilities\PlantNetResultFile.json");
            return await File.ReadAllTextAsync(path);
        }
    }
}

