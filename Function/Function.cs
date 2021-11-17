using Function.Services;
using Function.UseCases;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Function
{
    internal class Function
    {
        private readonly IHandleRequestData _handleRequestData;
        private ILogger _logger;

        public Function(IPlantAnimalService plantAnimalService, IHandleRequestData handleRequestData)
        {
            _handleRequestData = handleRequestData;
        }

        [OpenApiOperation(operationId: "post_plants", tags: new[] { "greeting" }, Summary = "Greetings", Description = "This shows a welcome message.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter("name", Type = typeof(string), In = ParameterLocation.Query, Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Summary = "The response", Description = "This returns the response")]

        [Function("plantcheck")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/plantcheck")] HttpRequestData request,
            FunctionContext executionContext)
        {
            _logger = executionContext.GetLogger("PlantCheck");
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var resultBody = await _handleRequestData.HandleRequest(request);
            
            // if content OK return OK and stuff
            var response = request.CreateResponse(HttpStatusCode.OK);
            //response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            const string fakeContent =
@"{
    ""Animal"": ""Alpaca"",
    ""ScientificName"": ""Test"",
}";
            await response.WriteStringAsync(resultBody);

            // if not OK return not OK

            // return
            return response;
        }
    }
}
