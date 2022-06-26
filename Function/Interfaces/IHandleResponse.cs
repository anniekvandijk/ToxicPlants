using Function.Models;
using Function.Models.Response;
using Microsoft.Azure.Functions.Worker.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Function.Interfaces
{
    internal interface IHandleResponse
    {
        Task<HttpResponseData> SetResponse(HttpRequestData request, List<PlantResponse> resultBody);
    }
}
