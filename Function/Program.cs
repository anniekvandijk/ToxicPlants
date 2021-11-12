using Function.MiddleWare;
using Function.Repository;
using Function.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using Function.Interfaces;
using Microsoft.Extensions.Logging;

namespace Function
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults(e =>
                {
                    e.UseMiddleware<ExceptionMiddleware>();
                })
                .ConfigureLogging(g =>
                {
                    g.AddSentry();
                })
                .ConfigureServices(s =>
                {
                    s.AddSingleton<HttpClient>();
                    s.AddTransient<IEnvironmentVariableService, EnvironmentVariableService>();
                    // When debugging, not always make a call to a plants Api.
                    // So use profile 'FunctionFakePlantCall' when 
                    // the call is not nessesary.
                    if (Environment.GetEnvironmentVariable("MOCK_PLANTCALL") == "True")
                    {
                        s.AddTransient<IPlantService, FakePlantService>();
                    } else
                    {
                        s.AddTransient<IPlantService, PlantService>();
                    }
                    s.AddTransient<IPlantRepository, PlantRepository>();
                    s.AddTransient<IAnimalRepository, AnimalRepository>();
                    s.AddTransient<IToxicPlantRepository, ToxicPlantRepository>();
                })
                .Build();

            host.Run();
        }
    }
}