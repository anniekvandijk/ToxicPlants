using Function.MiddleWare.ExceptionHandler;
using Function.Repository;
using Function.Services;
using Function.UseCases;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Function.Tests")]

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
                        s.AddSingleton<IPlantService, FakePlantService>();
                    }
                    else
                    {
                        s.AddSingleton<IPlantService, PlantNetService>();
                    }
                    // One instance of a service which gets all toxoc plants data and adds it to the repository
                    s.AddSingleton<IPlantAnimalRepository, PlantAnimalRepository>();
                    s.AddSingleton<IPlantAnimalService, PlantAnimalService>();
                    s.AddScoped<IHandleRequestData, HandleRequestDataPlantCheck>();
                    s.AddScoped<IPlantRepository, PlantRepository>();
                    s.AddScoped<IAnimalRepository, AnimalRepository>();
                })

                .Build();



            host.Run();
        }
    }
}