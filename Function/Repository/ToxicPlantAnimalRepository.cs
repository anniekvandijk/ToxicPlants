﻿using Function.Interfaces;
using Function.MiddleWare.ExceptionHandler;
using Function.Models;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
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

            if (plantAnimal.Animal.IsNullOrDefault())
                ProgramError.CreateProgramError(HttpStatusCode.InternalServerError, $"Animal can not be empty");
            else if (plantAnimal.ScientificClassification.IsNullOrDefault())
                ProgramError.CreateProgramError(HttpStatusCode.InternalServerError, $"ScientificClassification can not be empty");
            else if (plantAnimal.ScientificClassification == ScientificClassification.Species && string.IsNullOrEmpty(plantAnimal.Species?.Trim()))
                ProgramError.CreateProgramError(HttpStatusCode.InternalServerError, $"Species can not be empty");
            else if (plantAnimal.ScientificClassification == ScientificClassification.Genus && string.IsNullOrEmpty(plantAnimal.Genus?.Trim()))
                ProgramError.CreateProgramError(HttpStatusCode.InternalServerError, $"Genus can not be empty");
            else if (plantAnimal.ScientificClassification == ScientificClassification.Family && string.IsNullOrEmpty(plantAnimal.Family?.Trim()))
                ProgramError.CreateProgramError(HttpStatusCode.InternalServerError, $"Family can not be empty");
            else if (plantAnimal.HowToxic is < mimimumClass or > maximumClass)
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
