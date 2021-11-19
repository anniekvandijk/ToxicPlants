using System.Net;

namespace Function.Models.Response
{
    internal class ErrorResponse
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public int ErrorCode { get; set; }
        public string Message { get; set; }
    }
}
