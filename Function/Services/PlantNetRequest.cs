using Function.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace Function.Services
{
    internal class PlantNetRequest : IPlantRequest
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PlantNetRequest> _logger;

        public PlantNetRequest(HttpClient httpClient, ILogger<PlantNetRequest> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }


        public async Task<HttpResponseMessage> MakeRequest(HttpRequestMessage httpRequestMessage)
        {
            _logger.LogInformation("PlantNet Service called");
            
            return await _httpClient.SendAsync(httpRequestMessage);
        }
    }
}

