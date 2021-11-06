using Function.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;

namespace Function.Services
{
    public class FakePlantNetService : IPlantNetService
    {
        public async Task<string> GetPlantsAsync(RequestData data)
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Utilities\PlantNetResultFile.json");
            return await File.ReadAllTextAsync(path);
        }
    }
}

