using Function.Interfaces;
using Function.MiddleWare.ExceptionHandler;
using Function.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Function.Repository
{
    internal class ToxicPlantAnimalRepository : IToxicPlantAnimalRepository
    {
        private readonly List<ToxicPlantAnimal> _toxicPlantAnimals = new();

        public void Add(ToxicPlantAnimal plantAnimal)
        {
            const int mimimumClass = 1;
            const int maximumClass = 3;

            plantAnimal.Summary?.Trim();

            if (plantAnimal.HowToxic is < mimimumClass or > maximumClass)
                ProgramError.CreateProgramError(HttpStatusCode.InternalServerError, $"Toxicclass is not in range of {mimimumClass}-{maximumClass}");
            else _toxicPlantAnimals.Add(plantAnimal);

        }

        public List<ToxicPlantAnimal> Get() => _toxicPlantAnimals;

        public ToxicPlantAnimal GetById(string id) =>
            _toxicPlantAnimals.First(x => x.Id == id);

        public List<ToxicPlantAnimal> GetByAnimalName(Animal animal) =>
            _toxicPlantAnimals
                .Where(x => x.Animal == animal).ToList();

        public List<ToxicPlantAnimal> GetbyAnimalAndPlantName(Animal animal, Plant plant)
        {
            var list = new List<ToxicPlantAnimal>();

            var toxicForAnimal = GetByAnimalName(animal);

            foreach (var toxicPlantAnimal in toxicForAnimal)
            {
                switch (toxicPlantAnimal.ScientificClassification)
                {
                    case ScientificClassification.Species when toxicPlantAnimal.Species == plant.Species:
                    case ScientificClassification.Genus when toxicPlantAnimal.Genus == plant.Genus:
                    case ScientificClassification.Family when toxicPlantAnimal.Family == plant.Family:
                        list.Add(toxicPlantAnimal);
                        break;
                }
            }

            return list;
        }

    }
}
