using System.Text.Json;

namespace Function.Models.Response
{
    internal class PlantResponse
    {
        public Animal Animal { get; set; }
        public string PlantName { get; set; }
        public int HowToxic { get; set; }
        public string Reference { get; set; }
        public JsonElement PlantDetail { get; set; }
    }
}
