using Function.Repository;
using Function.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;

namespace Function
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(s =>
                {
                    s.AddSingleton<HttpClient>();
                    s.AddTransient<IEnvironmentVariableService, EnvironmentVariableService>();
                    // When debugging, not always make a call to PlantNet.
                    // So use profile 'FunctionFakePlantNet' when 
                    // the call is not nessesary.
                    if (Environment.GetEnvironmentVariable("PlantCall") == "False")
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