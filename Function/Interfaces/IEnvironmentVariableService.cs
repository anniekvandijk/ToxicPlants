namespace Function.Services
{
    public interface IEnvironmentVariableService
    {
        public string GetPlantUrl();
        public string GetSentryDsn();
    }
}
