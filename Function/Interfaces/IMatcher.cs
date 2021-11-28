using System.Collections.Generic;
using Function.Models;
using Function.Models.Response;

namespace Function.Interfaces
{
    internal interface IMatcher
    {
        List<ToxicPlantAnimal> MatchToxicPlantsForAnimals();
    }
}