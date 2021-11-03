using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Function.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Function
{
    public class Function
    {
        private readonly HttpClient _httpClient;
        private readonly IEnvironmentVariableService _environmentVariableService;
        public Function(HttpClient httpClient, IEnvironmentVariableService environmentVariableService)
        {
            _httpClient = httpClient;
            _environmentVariableService = environmentVariableService;
        }

        [Function("plantcheck")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData request,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("PlantCheck");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            // read the body and check content
            var responseBody = SortAllOut(request);

            // if content OK return OK and stuff
            var response = request.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            await response.WriteStringAsync(responseBody);

            // if not OK return not OK

            // return
            return response;
        }

        private string SortAllOut(HttpRequestData request)
        {

            return "Welcome to Azure Functions!";
        }

        public async Task<HttpResponseMessage> GetPlants()
        {
            var plantRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(_environmentVariableService.GetPlantNetUrl),
                Method = HttpMethod.Post,
            };

            return await _httpClient.SendAsync(plantRequest);
        }
    }
}
