using System;
using Function.Interfaces;
using Function.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CsvHelper;
using Function.MiddleWare.ExceptionHandler;
using Function.Models.Request;

namespace Function.Services
{
    internal class ToxicPlantAnimalService : IToxicPlantAnimalService
    {
        private readonly IToxicPlantAnimalRepository _toxicPlantAnimalRepository;
        private readonly ILogger<ToxicPlantAnimalService> _logger;

        public ToxicPlantAnimalService(IToxicPlantAnimalRepository toxicPlantAnimalRepository, ILogger<ToxicPlantAnimalService> logger)
        {
            _toxicPlantAnimalRepository = toxicPlantAnimalRepository;
            _logger = logger;
        }

        public void LoadToxicPlantAnimalData()
        {
            if (_toxicPlantAnimalRepository.Get().Count == 0)
                LoadFromFile();
        }

        public void LoadFromFile()
        {
            _logger.LogInformation("Loader toxicplants called");

            var path =
                Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidOperationException("File not found")
                    , @"Data\ToxicPlants.csv");

            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, new CultureInfo("nl-NL"));

            try
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    try
                    {
                        var record = csv.GetRecord<ToxicPlantAnimal>();
                        _toxicPlantAnimalRepository.Add(record);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Adding Toxic Plant record failed.");
                    }
                }
            }
            catch
            {
                ProgramError.CreateProgramError(HttpStatusCode.Conflict, "Reading toxicplant data error");
            }
        }
    }
}
