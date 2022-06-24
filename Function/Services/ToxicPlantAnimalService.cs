using CsvHelper;
using Function.Interfaces;
using Function.MiddleWare.ExceptionHandler;
using Function.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.IO;
using System.Net;

namespace Function.Services
{
    internal class ToxicPlantAnimalService : IToxicPlantAnimalService
    {
        private readonly IToxicPlantAnimalRepository _toxicPlantAnimalRepository;
        private readonly IFileHelper _fileHelper;
        private readonly ILogger<ToxicPlantAnimalService> _logger;

        public ToxicPlantAnimalService(IToxicPlantAnimalRepository toxicPlantAnimalRepository, ILogger<ToxicPlantAnimalService> logger, IFileHelper fileHelper)
        {
            _toxicPlantAnimalRepository = toxicPlantAnimalRepository;
            _fileHelper = fileHelper;
            _logger = logger;
        }

        public void LoadToxicPlantAnimalData()
        {
            if (_toxicPlantAnimalRepository.Get().Count != 0) return;

            var plants = _fileHelper.GetToxicPlantAnimalFileLocation("ToxicPlants_Plants_v2.0.csv");
            var reference = _fileHelper.GetToxicPlantAnimalFileLocation("ToxicPlants_Reference_v2.0.csv");

            using var plantsReader = new StreamReader(plants);
            using var plantsCsv = new CsvReader(plantsReader, new CultureInfo("nl-NL"));

            try
            {
                plantsCsv.Read();
                plantsCsv.ReadHeader();
                var lineNumber = 1;
                while (plantsCsv.Read())
                {
                    lineNumber++;
                    try
                    {
                        var record = plantsCsv.GetRecord<ToxicPlantAnimal>();
                        _toxicPlantAnimalRepository.Add(record);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Adding Toxic Plant record failed at line {lineNumber}");
                    }
                }
            }
            catch
            {
                ProgramError.CreateProgramError(HttpStatusCode.Conflict, "Reading toxicplant data error");
            }

            using var referenceReader = new StreamReader(reference);
            using var referenceCsv = new CsvReader(referenceReader, new CultureInfo("nl-NL"));

            try
            {
                referenceCsv.Read();
                referenceCsv.ReadHeader();
                var lineNumber = 1;
                while (referenceCsv.Read())
                {
                    lineNumber++;
                    try
                    {
                        var record = referenceCsv.GetRecord<InformationReference>();
                        _toxicPlantAnimalRepository.GetById(record.ToxicPlantId).InformationReferences.Add(record);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Adding Toxic Plant record failed at line {lineNumber}");
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
