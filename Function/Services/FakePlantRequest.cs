using Function.Interfaces;
using Function.Models.Request;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace Function.Services
{
    internal class FakePlantRequest : IPlantRequest
    {
        private readonly ILogger<FakePlantRequest> _logger;

        public FakePlantRequest(ILogger<FakePlantRequest> logger)
        {
            _logger = logger;
        }

        public async Task<HttpResponseMessage> MakeRequest(HttpRequestMessage httpRequestMessage)
        {
            _logger.LogInformation("Fake Plant Service called");

            var fileName = Environment.GetEnvironmentVariable("FAKE_PLANTREQUEST_FILENAME");

            var path =
                Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ??
                             throw new InvalidOperationException("File not found")
                    , "Utilities", fileName);
            
            var content =  await File.ReadAllTextAsync(path);

            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            responseMessage.Content = new StringContent(content);
            return responseMessage;

        }
    }
}

