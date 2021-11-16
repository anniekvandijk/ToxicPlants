using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Function.MiddleWare.ExceptionHandler
{
    internal class ExceptionHandlerMiddleware : IFunctionsWorkerMiddleware
    {
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            ILogger<ExceptionHandlerMiddleware> logger = context.GetLogger<ExceptionHandlerMiddleware>();

            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);

                var statuscode = HttpStatusCode.InternalServerError;

                if (ex.InnerException != null)
                {
                    var exceptionType = ex.InnerException.GetType();

                    if (exceptionType == typeof(RequestDataException))
                    {
                        statuscode = HttpStatusCode.BadRequest;
                    }
                }

                await SetResponse(context, logger, statuscode, ex);

            }
        }

        private static async Task SetResponse(FunctionContext context, ILogger<ExceptionHandlerMiddleware> logger, HttpStatusCode statusCode, Exception ex)
        {
            var request = context.GetHttpRequestData(logger);
            var response = request?.CreateResponse(statusCode);

#if DEBUG
            await response.WriteStringAsync(ex.StackTrace);
#else
            await response.WriteAsJsonAsync("not ok");
#endif
            context.SetHttpResponseData(response, logger);
        }
    }
}
