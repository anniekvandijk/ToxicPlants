using System.Net.Http;
using Function.Models.Request;
using System.Threading.Tasks;

namespace Function.Interfaces
{
    internal interface IPlantRequest
    {
        public Task<HttpResponseMessage> MakeRequest(HttpRequestMessage httpRequestMessage);
    }
}
