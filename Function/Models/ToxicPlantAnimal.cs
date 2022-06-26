using System.Collections.Generic;
using System.Text.Json;

namespace Function.Models
{
    internal class ToxicPlantAnimal
    {
        public string Id { get; set; }
        public Animal Animal { get; set; }
        public ScientificClassification ScientificClassification { get; set; }
        public string Species { get; set; }
        public string Genus { get; set; }
        public string Family { get; set; }
        public int HowToxic { get; set; }
        public string Summary { get; set; }
        public List<InformationReference> InformationReferences { get; set; } = new List<InformationReference>();

    }

    internal class InformationReference
    {
        public string ToxicPlantId { get; set; }
        public string Reference { get; set; }
        public string Information { get; set; } 

    }
}
