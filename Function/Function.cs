using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Function
{
    public class Function
    {
        private readonly HttpClient _httpClient;
        public Function(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [Function("plantcheck")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData request,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("PlantCheck");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            // read the body and check content

            // sent to plantnet

            // if content OK return OK and stuff
            var response = request.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            await response.WriteStringAsync("Welcome to Azure Functions!");

            // if not OK return not OK

            // return
            return response;
        }

        public async Task<string> GetPlants()
        {
            var PlantNetUrl = Environment.GetEnvironmentVariable("PlantNetUrl");
            
            var plantRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(PlantNetUrl),
                Method = HttpMethod.Post,
            };

            HttpResponseMessage plantnet = await _httpClient.SendAsync(plantRequest);
            return plantnet.Content.ReadAsStringAsync().Result;
        }
    }
}
