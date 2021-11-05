using Function.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Function.Repository
{
    public class ToxicPlantsRepository : IToxicPlantsRepository
    {
        public List<ToxicPlant> GetByAnimalName(Animal animal)
        {
            return GetTempList().SingleOrDefault(x => x.Animal == animal).ToxicPlants;
        }


        private List<AnimalToxicPlant> GetTempList()
        {
            var toxicPlantList = new List<AnimalToxicPlant>();

            var alpacalist = new List<ToxicPlant>();
            alpacalist.Add(new ToxicPlant { Name = "Adonis aestivalis", Animal = Animal.Alpaca, HowToxic = "very", Reference = "Alpacawereld" });
            alpacalist.Add(new ToxicPlant { Name = "Prunus serotina", Animal = Animal.Alpaca, HowToxic = "very", Reference = "Alpacawereld" });
            alpacalist.Add(new ToxicPlant { Name = "Rhodondendron", Animal = Animal.Alpaca, HowToxic = "very", Reference = "Alpacawereld" });
            alpacalist.Add(new ToxicPlant { Name = "Hyoscyamus niger", Animal = Animal.Alpaca, HowToxic = "very", Reference = "Alpacawereld" });
            var animalToxicPlant = new AnimalToxicPlant { Animal = Animal.Alpaca, ToxicPlants = alpacalist };

            toxicPlantList.Add(animalToxicPlant);

            return toxicPlantList;
        }
    }
}
