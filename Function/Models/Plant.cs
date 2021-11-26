using System.Text.Json;

namespace Function.Models
{
    internal class Plant
    {
        public string ScientificName { get; set; }
        public JsonElement PlantDetail { get; set; }
    }
}
