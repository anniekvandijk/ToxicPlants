using Function.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Function.Repository
{
    public class ToxicPlantRepository : IToxicPlantRepository
    {
        private readonly List<AnimalToxicPlant> animalToxicPlants;

        public ToxicPlantRepository()
        {
            animalToxicPlants = new();
            TempList();
        }

        public List<ToxicPlant> GetByAnimalName(Animal animal)
        {
            return animalToxicPlants
                .SingleOrDefault(x => x.Animal == animal)
                .ToxicPlants;
        }

        public ToxicPlant GetbyAnimalAndPlantName(Animal animal, Plant plant)
        {
            return GetByAnimalName(animal)
                .SingleOrDefault(x => x.Name == plant.Name);
        }

        private void TempList()
        {
            var alpacalist = new List<ToxicPlant>
            {
                new ToxicPlant { Name = "Adonis aestivalis", Animal = Animal.Alpaca, HowToxic = "very", Reference = "Alpacawereld" },
                new ToxicPlant { Name = "Prunus serotina", Animal = Animal.Alpaca, HowToxic = "very", Reference = "Alpacawereld" },
                new ToxicPlant { Name = "Rhodondendron", Animal = Animal.Alpaca, HowToxic = "very", Reference = "Alpacawereld" },
                new ToxicPlant { Name = "Hyoscyamus niger", Animal = Animal.Alpaca, HowToxic = "very", Reference = "Alpacawereld" }
            };

            var animalToxicPlant = new AnimalToxicPlant { Animal = Animal.Alpaca, ToxicPlants = alpacalist };

            animalToxicPlants.Add(animalToxicPlant);
        }
    }
}
