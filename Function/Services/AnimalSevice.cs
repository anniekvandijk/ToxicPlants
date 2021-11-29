using System;
using System.Linq;
using System.Net;
using Function.Interfaces;
using Function.MiddleWare.ExceptionHandler;
using Function.Models;
using Function.Models.Request;

namespace Function.Services
{
    internal class AnimalSevice : IAnimalSevice
    {
        private readonly IAnimalRepository _animalRepository;

        public AnimalSevice(IAnimalRepository animalRepository)
        {
            _animalRepository = animalRepository;
        }

        public void AddAnimals(RequestData data)
        {
            var animals = data.Parameters.Where(x => x.Name.ToLower() == "animal").ToList();

            if (animals.Count == 0)
                ProgramError.CreateProgramError(HttpStatusCode.BadRequest, "No animal parameter in request");

            foreach (var animal in animals)
            {
                if (Enum.TryParse(animal.Data, true, out Animal animalEnum))
                    _animalRepository.Add(animalEnum);
                else
                    ProgramError.CreateProgramError(HttpStatusCode.BadRequest, "No animal or unsupported animal in request");
            }
        }
    }
}
