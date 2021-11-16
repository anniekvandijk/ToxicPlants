using System.Collections.Generic;

namespace Function.Models
{
    internal class Plant
    {
        public string ScientificName { get; set; }
        public List<PlantCommonName> CommonNames { get; set; }
        public double Score { get; set; }
    }
}
