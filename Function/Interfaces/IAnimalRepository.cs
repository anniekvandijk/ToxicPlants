using Function.Models;
using System.Collections.Generic;

namespace Function.Interfaces
{
    internal interface IAnimalRepository
    {
        void Add(Animal animal);
        List<Animal> Get();
    }
}
