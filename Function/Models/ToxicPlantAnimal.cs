namespace Function.Models
{
    internal class ToxicPlantAnimal
    {
        public Animal Animal { get; set; }
        public string PlantName { get; set; }
        public ScientificClassification ScientificClassification { get; set;}
        public int HowToxic { get; set; }
        public string Reference { get; set; }
        public string ExtraInformation { get; set; }

    }
}
