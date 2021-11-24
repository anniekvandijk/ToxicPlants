using Function.Interfaces;
using Function.Models;
using System.Collections.Generic;

namespace Function.Repository
{
    internal class AnimalRepository : IAnimalRepository
    {
        private readonly List<Animal> _animals = new();

        public void Add(Animal animal)
        {
            if (!_animals.Contains(animal)) _animals.Add(animal);
        }

        public List<Animal> Get() => _animals;

    }
}
