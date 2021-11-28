using System.Collections.Generic;
using Function.Models.Response;

namespace Function.Interfaces
{
    internal interface IMatcher
    {
        List<ToxicPlantResponse> MatchToxicPlantsForAnimals();
    }
}