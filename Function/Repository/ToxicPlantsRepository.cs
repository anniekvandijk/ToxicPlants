using Function.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Function.Repository
{
    public class ToxicPlantsRepository : IToxicPlantsRepository
    {
        private readonly List<AnimalToxicPlant> animalToxicPlants;

        public ToxicPlantsRepository()
        {
            animalToxicPlants = new();
            TempList();
        }

        public List<ToxicPlant> GetByAnimalName(Animal animal)
        {
            return animalToxicPlants.SingleOrDefault(x => x.Animal == animal).ToxicPlants;
        }

        public ToxicPlant GetToxicPlant(Animal animal, string plantName)
        {
            return GetByAnimalName(animal).SingleOrDefault(x => x.Name == plantName);
        }

        private static void TempList()
        {
            var alpacalist = new List<ToxicPlant>();
            alpacalist.Add(new ToxicPlant { Name = "Adonis aestivalis", Animal = Animal.Alpaca, HowToxic = "very", Reference = "Alpacawereld" });
            alpacalist.Add(new ToxicPlant { Name = "Prunus serotina", Animal = Animal.Alpaca, HowToxic = "very", Reference = "Alpacawereld" });
            alpacalist.Add(new ToxicPlant { Name = "Rhodondendron", Animal = Animal.Alpaca, HowToxic = "very", Reference = "Alpacawereld" });
            alpacalist.Add(new ToxicPlant { Name = "Hyoscyamus niger", Animal = Animal.Alpaca, HowToxic = "very", Reference = "Alpacawereld" });
            
            var animalToxicPlant = new AnimalToxicPlant { Animal = Animal.Alpaca, ToxicPlants = alpacalist };

            animalToxicPlants.Add(animalToxicPlant);
        }
    }
}
