using System.Collections.Generic;

namespace Function.Models.Response
{
    internal class PlantResponse
    {
        public string Animal { get; set; }
        public string ScientificClassification { get; set; }
        public int HowToxic { get; set; }
        public string Summary { get; set; }
        public List<InformationReference> InformationReferences { get; set; } = new List<InformationReference>();
        public string ScientificName { get; set; }
        public List<string> CommonNames { get; set; }
        public double Score { get; set; }
        public List<string> ImagesUrls { get; set; }
    }

    internal class InformationReference
    {
        public string Reference { get; set; }
        public string Information { get; set; }

    }
}
