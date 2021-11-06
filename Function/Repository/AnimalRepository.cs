using Function.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Function.Repository
{
    public class AnimalRepository : IAnimalRepository
    {
        private static readonly List<Animal> animals = new();
     
        public void Add(string animal)
        {
            if (Enum.TryParse(animal, true, out Animal animalEnum))
            {
                animals.Add(animalEnum);
            }
            else
            {
                // error?
            }
        }

        public void AddAll(RequestData data)
        {
            var animals = data.Parameters.Where(x => x.Name == "animal");

            foreach (var animal in animals)
            {
                Add(animal.Data);
            }
        }

        public List<Animal> GetAll()
        {
            return animals;
        }
    }
}
