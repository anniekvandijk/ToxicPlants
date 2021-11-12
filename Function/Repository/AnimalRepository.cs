using Function.Models;
using System.Collections.Generic;
using Function.Interfaces;

namespace Function.Repository
{
    public class AnimalRepository : IAnimalRepository
    {
        private readonly List<Animal> _animals;

        public AnimalRepository()
        {
            _animals = new();
        }
     
        public void Add(Animal animal)
        {
            if (!_animals.Contains(animal)) _animals.Add(animal);
        }

        public List<Animal> Get()
        {
            return _animals;
        }
    }
}
