using Function.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Threading.Tasks;

namespace Function
{
    internal class Function
    {
        private readonly IHandleRequest _handleRequest;
        private readonly IHandleResponse _handleResponse;
        private ILogger _logger;

        public Function(IHandleRequest handleRequest, IHandleResponse handleResponse)
        {
            _handleRequest = handleRequest;
            _handleResponse = handleResponse;
        }

        [Function("plantcheck")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/plantcheck")] HttpRequestData request,
            FunctionContext executionContext)
        {
            _logger = executionContext.GetLogger("PlantCheck");
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            // If something goes wrong, all is handled by the ExceptionHandlerMiddleware

            var resultBody = await _handleRequest.Handle(request);
            return await _handleResponse.SetResponse(request, resultBody);
        }
    }
}
