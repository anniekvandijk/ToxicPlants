using Function.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Function.Utilities;

namespace Function
{
    internal class Function
    {
        private readonly IHandleRequest _handleRequest;
        private readonly IHandleResponse _handleResponse;
        private readonly IMatchData _matchData;
        private ILogger _logger;

        public Function(IHandleRequest handleRequest, IHandleResponse handleResponse, IMatchData matchData)
        {
            _handleRequest = handleRequest;
            _handleResponse = handleResponse;
            _matchData = matchData;
        }

        [Function("plantcheck")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/plantcheck")] HttpRequestData request,
            FunctionContext executionContext)
        {
            _logger = executionContext.GetLogger("PlantCheck");
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            // If something goes wrong, all is handled by the ExceptionHandlerMiddleware
            await _handleRequest.CollectData(request);
            var result = _matchData.MatchToxicPlantsForAnimals();
            return await _handleResponse.SetResponse(request, result);
        }
    }
}
