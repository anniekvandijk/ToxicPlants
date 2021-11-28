using System.Text.Json;

namespace Function.Models.Response
{
    internal class ToxicPlantResponse
    {
        public Animal Animal { get; set; }
        public string PlantName { get; set; }
        public int HowToxic { get; set; }
        public string Reference { get; set; }
        public string ExtraInformation { get; set; }
        public JsonElement PlantDetail { get; set; }
    }
}
