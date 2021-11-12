using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Function.MiddleWare
{
    public class ExceptionMiddleware : IFunctionsWorkerMiddleware
    {
        private ILogger<ExceptionMiddleware> logger;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
        {
            this.logger = logger;
        }

        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            HttpRequestData httpRequestData = null;

            try
            {
                httpRequestData = context.GetHttpRequestData();
                // Code before function execution here
                await next(context);
                // Code after function execution here
            }
            catch (Exception ex)
            {
                logger.LogError(ex, string.Empty);


                HttpResponseData response = httpRequestData.CreateResponse(HttpStatusCode.BadRequest);
                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

                await response.WriteStringAsync("Not ok");
            }
        }
    }
}
