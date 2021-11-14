using Function.MiddleWare;
using Function.Repository;
using Function.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using Function.MiddleWare.ExceptionHandler;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
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
                    e.UseMiddleware<ExceptionHandlerMiddleware>();
                    e.UseNewtonsoftJson();
                })
                .ConfigureOpenApi()
                .ConfigureLogging(g =>
                {
                    g.AddSentry();
                })
                .ConfigureServices(s =>
                {
                    s.AddSingleton<HttpClient>();
                    // When debugging, not always make a call to a plants Api.
                    // So use profile 'FunctionFakePlantCall' when 
                    // the call is not nessesary.
                    if (Environment.GetEnvironmentVariable("MOCK_PLANTCALL") == "True")
                    {
                        s.AddTransient<IPlantService, FakePlantService>();
                    } else
                    {
                        s.AddTransient<IPlantService, PlantNetService>();
                    }
                    s.AddTransient<IPlantRepository, PlantRepository>();
                    s.AddTransient<IAnimalRepository, AnimalRepository>();
                    s.AddTransient<IPlantAnimalRepository, PlantAnimalRepository>();
                })

                .Build();

            host.Run();
        }
    }
}