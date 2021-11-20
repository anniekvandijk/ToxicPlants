using Function.Interfaces;
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
using System.Text.Json;
using System.Text.Json.Serialization;

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
                .ConfigureLogging(g =>
                {
                    g.AddSentry();
                })
                .ConfigureServices(s =>
                {
                    // TODO: NOT WORKING: https://github.com/Animundo/ToxicPlants/issues/7
                    s.AddControllers().AddJsonOptions(x =>
                    {
                        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: false));
                        x.JsonSerializerOptions.IgnoreNullValues = true;
                    });

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
                    s.AddSingleton<IToxicPlantAnimalRepository, ToxicPlantAnimalRepository>();

                    // Load this one instance of ToxicPlantAnimalRepository
                    var toxicPlantAnimalRepositoryProvider = s.BuildServiceProvider();
                    var toxicPlantAnimalRepository = toxicPlantAnimalRepositoryProvider.GetService<IToxicPlantAnimalRepository>();

                    // Use it to add to the ToxicPlantAnimalService
                    s.AddSingleton<IToxicPlantAnimalService>(x =>
                        new ToxicPlantAnimalService(toxicPlantAnimalRepository)
                    );

                    // Load the ToxicPlantAnimalService to Load initial data
                    var toxicPlantAnimalServiceProvider = s.BuildServiceProvider();
                    var toxicPlantAnimalService = toxicPlantAnimalServiceProvider.GetService<IToxicPlantAnimalService>();
                    toxicPlantAnimalService.LoadPlantAnimalData();

                    s.AddScoped<IHandleRequest, HandleRequest>();
                    s.AddScoped<IPlantRepository, PlantRepository>();
                    s.AddScoped<IAnimalRepository, AnimalRepository>();
                    s.AddScoped<IHandleResponse, HandleResponse>();
                })

                .Build();



            host.Run();
        }
    }
}