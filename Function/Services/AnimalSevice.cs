using System;
using System.Linq;
using System.Net;
using Function.Interfaces;
using Function.MiddleWare.ExceptionHandler;
using Function.Models;
using Function.Models.Request;
using Microsoft.Extensions.Logging;

namespace Function.Services
{
    internal class AnimalSevice : IAnimalSevice
    {
        private readonly ILogger<AnimalSevice> _logger;
        private readonly IAnimalRepository _animalRepository;

        public AnimalSevice(ILogger<AnimalSevice> logger, IAnimalRepository animalRepository)
        {
            _logger = logger;
            _animalRepository = animalRepository;
        }

        public void AddAnimals(RequestData data)
        {
            var animals = data.Parameters.Where(x => x.Name == "animal").ToList();

            if (animals.Count == 0)
            {
                ProgramError.CreateProgramError(HttpStatusCode.BadRequest, "No animal found");
            }

            foreach (var animal in animals)
            {
                if (Enum.TryParse(animal.Data, true, out Animal animalEnum))
                {
                    _animalRepository.Add(animalEnum);
                }
                else
                {
                    ProgramError.CreateProgramError(HttpStatusCode.BadRequest, "Animal not supported");
                }
            }
            _logger.LogInformation($"Added {animals.Count} animals to AnimalRepository.");
        }
    }
}
