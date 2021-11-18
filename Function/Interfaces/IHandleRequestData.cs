using Microsoft.Azure.Functions.Worker.Http;
using System.Threading.Tasks;

namespace Function.Interfaces
{
    internal interface IHandleRequestData
    {
        Task<string> HandleRequest(HttpRequestData request);
    }
}
