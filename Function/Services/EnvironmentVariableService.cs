using Function.Services;
using System;

namespace Function
{
    public class EnvironmentVariableService : IEnvironmentVariableService
    {

        public string GetPlantUrl()
            => Get("plantUrl");

        public string GetSentryDsn()
            => Get("SentryDsn");

        private static string Get(string name)
        {
            return Environment.GetEnvironmentVariable(name);
        }

    }
}
