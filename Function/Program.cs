using Function.Interfaces;
using Function.MiddleWare.ExceptionHandler;
using Function.Repository;
using Function.Services;
using Function.UseCases;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
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
                    /* Global Exception handling.
                     All errors are handled by the ExceptionHandlerMiddleWare.
                     */
                    e.UseMiddleware<ExceptionHandlerMiddleware>();
                })
                .ConfigureLogging(g =>
                {
                    /*
                    When testing local. Do not alwaiy sent logging to Sentry.
                    In local.settings.json, if "SENT_TO_SENTRY": true,
                    Sentry is added to the logger.
                    */
                    if (Convert.ToBoolean(Environment.GetEnvironmentVariable("SENT_TO_SENTRY")))
                    {
                        g.AddSentry(x => 
                            x.MinimumEventLevel = LogLevel.Warning);
                    }
                })
                .ConfigureServices(s =>
                {
                    /*
                    The response of all http calls is altered in this way:
                    - Enums are translated to string values
                    - Null values are not returned
                    TODO: NOT WORKING: https://github.com/Animundo/ToxicPlants/issues/7
                    */
                    s.AddControllers().AddJsonOptions(x =>
                    {
                        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                        x.JsonSerializerOptions.IgnoreNullValues = true;
                    });

                    s.AddSingleton<HttpClient>();
                    /*
                    When debugging, not always make a call to a plants Api.
                    So use profile 'FunctionFakePlantCall' when the call is not nessesary.
                    */
                    if (Convert.ToBoolean(Environment.GetEnvironmentVariable("MOCK_PLANTCALL")))
                    {
                        s.AddSingleton<IPlantService, FakePlantService>();
                    }
                    else
                    {
                        s.AddSingleton<IPlantService, PlantNetService>();
                    }
                    // One instance of a service which gets all toxoc plants data and adds it to the repository
                    s.AddSingleton<IToxicPlantAnimalRepository, ToxicPlantAnimalRepository>();
                    var toxicPlantAnimalRepositoryProvider = s.BuildServiceProvider();
                    var toxicPlantAnimalRepository = toxicPlantAnimalRepositoryProvider.GetService<IToxicPlantAnimalRepository>();

                    s.AddSingleton<IToxicPlantAnimalService>(x =>
                        new ToxicPlantAnimalService(toxicPlantAnimalRepository, 
                            x.GetService<ILoggerFactory>().CreateLogger<ToxicPlantAnimalService>())
                    );
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