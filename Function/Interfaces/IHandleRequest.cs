using System.Threading.Tasks;
using System.IO;
using Microsoft.Azure.Functions.Worker.Http;

namespace Function.Interfaces
{
    internal interface IHandleRequest
    {
        Task CollectData(Stream requestBody);
    }
}
