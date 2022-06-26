using System.Collections.Generic;
using System.Text.Json;

namespace Function.Models
{
    internal class Plant
    {
        public string Species { get; set; }
        public string Genus { get; set; }
        public string Family { get; set; }
        public string ScientificName { get; set; }
        public List<string> CommonNames { get; set; }
        public double Score { get; set; }
        public List <string> ImagesUrls { get; set; }
    }
}
