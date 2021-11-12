using System.Collections.Generic;
using Function.Models;

namespace Function.Interfaces
{
    public interface IAnimalRepository
    {
        public void Add(Animal animal);

        public List<Animal> Get();
    }
}
