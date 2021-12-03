using Function.Interfaces;
using Function.Models.Request;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Function.Services
{
    internal class FakePlantRequest : IPlantRequest
    {
        private readonly ILogger<FakePlantRequest> _logger;

        public FakePlantRequest(ILogger<FakePlantRequest> logger)
        {
            _logger = logger;
        }

        public async Task<string> GetPlantsAsync(RequestData data)
        {
            _logger.LogInformation("Fake Plant Service called");

            var toxic = data.Parameters.First(x => x.Name == "toxic").Data;

            string fileName;
            if (toxic == "true")
            {
                fileName = "PlantNetToxic.json";
            }
            else
            {
                fileName = "PlantNetNonToxic.json";
            }

            var path =
                Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ??
                             throw new InvalidOperationException("File not found")
                    , "Utilities", fileName);

            return await File.ReadAllTextAsync(path);
        }
    }
}

