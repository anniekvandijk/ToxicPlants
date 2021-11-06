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
                    if (Environment.GetEnvironmentVariable("PlantNet") == "False")
                    {
                        s.AddTransient<IPlantNetService, FakePlantNetService>();
                    } else
                    {
                        s.AddTransient<IPlantNetService, PlantNetService>();
                    }
                    s.AddTransient<IPlantNetRepository, PlantNetRepository>();
                    s.AddTransient<IAnimalRepository, AnimalRepository>();
                    s.AddTransient<IToxicPlantsRepository, ToxicPlantsRepository>();
                })
                .Build();

            host.Run();
        }
    }
}