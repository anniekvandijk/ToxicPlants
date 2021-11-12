using System;
using Function.Interfaces;

namespace Function.Services
{
    public class EnvironmentVariableService : IEnvironmentVariableService
    {

        public string GetPlantUrl()
            => Get("PLANT_URL");

        private static string Get(string name)
        {
            return Environment.GetEnvironmentVariable(name);
        }

    }
}
