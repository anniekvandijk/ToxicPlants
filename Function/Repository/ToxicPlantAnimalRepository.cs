using System;
using Function.Interfaces;
using Function.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Function.MiddleWare.ExceptionHandler;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

namespace Function.Repository
{
    internal class ToxicPlantAnimalRepository : IToxicPlantAnimalRepository
    {
        private readonly List<ToxicPlantAnimal> _toxicPlantAnimals = new();

        public void Add(ToxicPlantAnimal plantAnimal)
        {
            const int mimimumClass = 1;
            const int maximumClass = 5;
            var plantName = plantAnimal.PlantName?.Trim();
            var reference = plantAnimal.Reference?.Trim();

            if (plantAnimal.HowToxic is < mimimumClass or > maximumClass)
            {
                ProgramError.CreateProgramError(HttpStatusCode.InternalServerError, $"Toxicclass is not in range of {mimimumClass}-{maximumClass}");
            } 
            else if (string.IsNullOrEmpty(plantName))
            {
                ProgramError.CreateProgramError(HttpStatusCode.InternalServerError, $"PlantName can not be empty");
            } 
            else if (string.IsNullOrEmpty(reference))
            {
                ProgramError.CreateProgramError(HttpStatusCode.InternalServerError, $"Reference can not be empty");
            } 
            else if (plantAnimal.Animal.IsNullOrDefault())
            {
                ProgramError.CreateProgramError(HttpStatusCode.InternalServerError, $"Animal can not be empty");
            }
            else
            {
                _toxicPlantAnimals.Add(plantAnimal);
            }
        }

        public List<ToxicPlantAnimal> Get() => _toxicPlantAnimals;

        public List<ToxicPlantAnimal> GetByAnimalName(Animal animal) =>
            _toxicPlantAnimals
                .Where(x => x.Animal == animal).ToList();

        public List<ToxicPlantAnimal> GetbyAnimalAndPlantName(Animal animal, Plant plant) =>
            _toxicPlantAnimals.Where(x => x.Animal == animal && x.PlantName == plant.ScientificName).ToList();
    }
}
