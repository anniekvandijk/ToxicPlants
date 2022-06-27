using Function.Models;
using Function.Models.Response;
using System.Collections.Generic;

namespace Function.Interfaces
{
    internal interface IMatchData
    {
        List<PlantResponse> MatchToxicPlantsForAnimals();
    }
}