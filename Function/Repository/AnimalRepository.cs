using Function.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Function.Repository
{
    public class AnimalRepository : IAnimalRepository
    {
        private readonly List<Animal> animals;

        public AnimalRepository()
        {
            animals = new();
        }
     
        public void Add(string animal)
        {
            if (Enum.TryParse(animal, true, out Animal animalEnum))
            {
                if (!animals.Contains(animalEnum)) animals.Add(animalEnum);
            }
            else
            {
                throw new ArgumentException("Animal not supported");
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
