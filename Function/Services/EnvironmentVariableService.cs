using Function.Services;
using System;

namespace Function
{
    public class EnvironmentVariableService : IEnvironmentVariableService
    {

        public string GetPlantUrl()
            => Get("plantUrl");

        private static string Get(string name)
        {
            return Environment.GetEnvironmentVariable(name);
        }
    }
}
