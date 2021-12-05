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

            var path = _fileHelper.GetToxicPlantAnimalFileLocation();

            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, new CultureInfo("nl-NL"));

            try
            {
                csv.Read();
                csv.ReadHeader();
                var lineNumber = 1;
                while (csv.Read())
                {
                    lineNumber++;
                    try
                    {
                        var record = csv.GetRecord<ToxicPlantAnimal>();
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
        }
    }
}
