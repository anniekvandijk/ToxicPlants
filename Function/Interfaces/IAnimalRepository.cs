using Function.Models;
using System.Collections.Generic;

namespace Function.Repository
{
    public interface IAnimalRepository
    {
        public void Add(string animal);

        public void AddAll(RequestData data);

        public List<Animal> GetAll();
    }
}
