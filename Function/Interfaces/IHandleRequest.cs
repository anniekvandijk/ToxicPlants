using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Http;

namespace Function.Interfaces
{
    internal interface IHandleRequest
    {
        Task CollectData(HttpRequestData request);
    }
}