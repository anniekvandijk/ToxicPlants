using Function.Models;
using System.Collections.Generic;
using Function.Interfaces;

namespace Function.Repository
{
    public class AnimalRepository : IAnimalRepository
    {
        private readonly List<Animal> animals;

        public AnimalRepository()
        {
            animals = new();
        }
     
        public void Add(Animal animal)
        {
            if (!animals.Contains(animal)) animals.Add(animal);
        }

        public List<Animal> Get()
        {
            return animals;
        }
    }
}
