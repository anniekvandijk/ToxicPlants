using Microsoft.Azure.Functions.Worker.Http;
using System.Threading.Tasks;

namespace Function.Interfaces
{
    internal interface IHandleRequest
    {
        Task<string> HandleRequest(HttpRequestData request);
    }
}
