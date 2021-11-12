using System;
using Function.Interfaces;

namespace Function.Services
{
    public class EnvironmentVariableService : IEnvironmentVariableService
    {

        public string GetPlantUrl()
            => Get("PLANT_URL");

        public string GetSentryDsn()
            => Get("SENTRY_DSN");

        private static string Get(string name)
        {
            return Environment.GetEnvironmentVariable(name);
        }

    }
}
