using Function.Models;
using System.Collections.Generic;

namespace Function.Repository
{
    public interface IAnimalRepository
    {
        public void Add(Animal animal);

        public List<Animal> Get();
    }
}
