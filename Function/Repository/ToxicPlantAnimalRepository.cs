using System;
using Function.Interfaces;
using Function.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Function.MiddleWare.ExceptionHandler;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Extensions.Logging;

namespace Function.Repository
{
    internal class ToxicPlantAnimalRepository : IToxicPlantAnimalRepository
    {
        private readonly List<ToxicPlantAnimal> _toxicPlantAnimals = new();

        public void Add(ToxicPlantAnimal plantAnimal)
        {
            const int mimimumClass = 1;
            const int maximumClass = 3;

            if (plantAnimal.HowToxic is < mimimumClass or > maximumClass) 
                ProgramError.CreateProgramError(HttpStatusCode.InternalServerError, $"Toxicclass is not in range of {mimimumClass}-{maximumClass}");
            else if (string.IsNullOrEmpty(plantAnimal.Species?.Trim())) 
                ProgramError.CreateProgramError(HttpStatusCode.InternalServerError, $"Species can not be empty");
            else if (string.IsNullOrEmpty(plantAnimal.Reference?.Trim())) 
                ProgramError.CreateProgramError(HttpStatusCode.InternalServerError, $"Reference can not be empty");
            else if (plantAnimal.Animal.IsNullOrDefault()) 
                ProgramError.CreateProgramError(HttpStatusCode.InternalServerError, $"Animal can not be empty");
            else _toxicPlantAnimals.Add(plantAnimal);
            
        }

        public List<ToxicPlantAnimal> Get() => _toxicPlantAnimals;

        public List<ToxicPlantAnimal> GetByAnimalName(Animal animal) =>
            _toxicPlantAnimals
                .Where(x => x.Animal == animal).ToList();

        public List<ToxicPlantAnimal> GetbyAnimalAndPlantName(Animal animal, Plant plant)
        {
            var list = new List<ToxicPlantAnimal>();

            var toxicForAnimal = GetByAnimalName(animal);
            
            foreach (var toxicPlantAnimal in toxicForAnimal)
            {
                if (toxicPlantAnimal.ScientificClassification == ScientificClassification.Species && toxicPlantAnimal.Species == plant.Species)
                {
                    list.Add(toxicPlantAnimal);
                }
                if (toxicPlantAnimal.ScientificClassification == ScientificClassification.Genus && toxicPlantAnimal.Genus == plant.Genus)
                {
                    list.Add(toxicPlantAnimal);
                }
                if (toxicPlantAnimal.ScientificClassification == ScientificClassification.Family && toxicPlantAnimal.Family == plant.Family)
                {
                    list.Add(toxicPlantAnimal);
                }
            }

            return list;
        }

    }
}
