using Function.Models;
using System.Collections.Generic;

namespace Function.Interfaces
{
    internal interface IMatchData
    {
        List<ToxicPlantAnimal> MatchToxicPlantsForAnimals();
    }
}