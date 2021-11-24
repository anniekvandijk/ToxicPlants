using Microsoft.Azure.Functions.Worker.Http;
using System.Threading.Tasks;

namespace Function.Interfaces
{
    internal interface IHandleResponse
    {
        Task<HttpResponseData> SetResponse(HttpRequestData request, string resultBody);
    }
}
