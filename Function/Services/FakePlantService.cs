﻿using Function.Interfaces;
using Function.Models.Request;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Function.Services
{
    internal class FakePlantService : IPlantService
    {
        private readonly ILogger<FakePlantService> _logger;

        public FakePlantService(ILogger<FakePlantService> logger)
        {
            _logger = logger;
        }

        public async Task<string> GetPlantsAsync(RequestData data)
        {
            _logger.LogInformation("Fake Plant Service called");

            var path =
                Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidOperationException("File not found")
                    , @"Utilities\PlantNetResultFile.json");
            return await File.ReadAllTextAsync(path);
        }
    }
}

