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
        public async Task<List<Plant>> GetPlantsAsync(RequestData data)
        {
            var plantList = new List<Plant>();

            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Utilities\PlantNetResultFile.json");
            var fileContent = await File.ReadAllTextAsync(path);

            var json = JsonConvert.DeserializeObject(fileContent).ToString();
            JObject jsonObject = JObject.Parse(json);
            JArray results = (JArray)jsonObject["results"];
            foreach (var result in results)
            {
                var plant = new Plant
                {
                    Name = (string)result["species"]["scientificName"],
                    Score = (double)result["score"]
                };
                plantList.Add(plant);
            }

            return plantList;
        }
    }
}

