using Function.Models;
using System.Collections.Generic;

namespace Function.Interfaces
{
    internal interface IMatcher
    {
        List<ToxicPlantAnimal> MatchToxicPlantsForAnimals();
    }
}