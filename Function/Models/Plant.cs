using System.Text.Json;

namespace Function.Models
{
    internal class Plant
    {
        public string ScientificName { get; set; }
        public string Species { get; set; }
        public string Genus { get; set; }
        public string Family { get; set; }
        public JsonElement PlantDetail { get; set; }
    }
}
