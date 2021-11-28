using System.Collections.Generic;
using Microsoft.Azure.Functions.Worker.Http;
using System.Threading.Tasks;
using Function.Models.Response;

namespace Function.Interfaces
{
    internal interface IHandleResponse
    {
        Task<HttpResponseData> SetResponse(HttpRequestData request, List<ToxicPlantResponse> resultBody);
    }
}
