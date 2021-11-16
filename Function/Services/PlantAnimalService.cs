using System.Collections.Generic;
using Function.Models;
using Function.Repository;

namespace Function.Services
{
    internal interface IPlantAnimalService
    {
        void LoadPlantAnimalData();
    }

    internal class PlantAnimalService : IPlantAnimalService
    {
        private readonly IPlantAnimalRepository _plantAnimalRepository;

        public PlantAnimalService(IPlantAnimalRepository plantAnimalRepository)
        {
            _plantAnimalRepository = plantAnimalRepository;
        }

        public void LoadPlantAnimalData()
        {
            var plantAnimalList = new List<PlantAnimal>
                {

                    new()
                    {
                        PlantName = "Adonis aestivalis", Animal = Animal.Alpaca, HowToxic = "very",
                        Reference = "Alpacawereld"
                    },
                    new()
                    {
                        PlantName = "Prunus serotina", Animal = Animal.Alpaca, HowToxic = "very",
                        Reference = "Alpacawereld"
                    },
                    new()
                    {
                        PlantName = "Rhodondendron", Animal = Animal.Alpaca, HowToxic = "very",
                        Reference = "Alpacawereld"
                    },
                    new()
                    {
                        PlantName = "Hyoscyamus niger", Animal = Animal.Alpaca, HowToxic = "very",
                        Reference = "Alpacawereld"
                    }
                };

            foreach (var plantAnimal in plantAnimalList)
            {
                _plantAnimalRepository.Add(plantAnimal);
            }
        }
    }
}
