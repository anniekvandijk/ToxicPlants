using Function.Models;
using Microsoft.Azure.Functions.Worker.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Function.Interfaces
{
    internal interface IHandleResponse
    {
        Task<HttpResponseData> SetResponse(HttpRequestData request, List<ToxicPlantAnimal> resultBody);
    }
}
