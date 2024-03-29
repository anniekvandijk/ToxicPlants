﻿using Function.Interfaces;
using Function.MiddleWare.ExceptionHandler;
using Function.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;

namespace Function.Repository
{
    internal class PlantRepository : IPlantRepository
    {
        private readonly List<Plant> _plants = new();
        private readonly ILogger<PlantRepository> _logger;

        public PlantRepository(ILogger<PlantRepository> logger)
        {
            _logger = logger;
        }

        public List<Plant> Get() => _plants;

        public void Add(Plant plant)
        {
            if (string.IsNullOrEmpty(plant.Species?.Trim()))
                ProgramError.CreateProgramError(HttpStatusCode.InternalServerError, $"Species can not be empty");
            else if (string.IsNullOrEmpty(plant.Genus?.Trim()))
                ProgramError.CreateProgramError(HttpStatusCode.InternalServerError, $"Genus can not be empty");
            else if (string.IsNullOrEmpty(plant.Family?.Trim()))
                ProgramError.CreateProgramError(HttpStatusCode.InternalServerError, $"Family can not be empty");

            var plantsInRepo = Get();
            if (!plantsInRepo.Exists(x => x.Species == plant.Species))
            {
                _plants.Add(plant);
            }
            else
            {
                _logger.LogCritical("Plant with same Species was not added to the Plant repository. Plantname = {_}", plant.Species);
            }

        }
    }
}
