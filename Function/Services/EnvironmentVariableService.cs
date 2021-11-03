using Function.Services;
using System;

namespace Function
{
    public class EnvironmentVariableService : IEnvironmentVariableService
    {

        private string plantNetUrl;
        public string GetPlantNetUrl { get => plantNetUrl; set => plantNetUrl = Get("plantNetUrl"); }

        private static string Get(string name)
        {
            return Environment.GetEnvironmentVariable(name);
        }
    }
}
