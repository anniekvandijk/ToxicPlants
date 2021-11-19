using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
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

                    // Request is not valid
                    if (exceptionType == typeof(RequestException))
                    {
                        statuscode = HttpStatusCode.BadRequest;
                    }
                    await SetResponse(context, logger, statuscode, ex.InnerException);
                    
                    if (exceptionType == typeof(PlantCallException))
                    {
                        statuscode = HttpStatusCode.BadRequest;
                    }
                    await SetResponse(context, logger, statuscode, ex.InnerException);
                }
                else
                {
                    await SetResponse(context, logger, statuscode, ex);
                }
            }
        }

        private static async Task SetResponse(FunctionContext context, ILogger<ExceptionHandlerMiddleware> logger, HttpStatusCode statusCode, Exception ex)
        {
            var request = context.GetHttpRequestData(logger);
            var response = request.CreateResponse(statusCode);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

#if DEBUG
            var stackTrace = ex.StackTrace;
#else
            string stackTrace = null;
#endif
            var body = new ExceptionResponse
            {
                HttpStatusCode = statusCode,
                Message = ex.Message,
                ErrorCode = ex.HResult
            };

            await response.WriteStringAsync(JsonSerializer.Serialize(body));
            context.SetHttpResponseData(response, logger);
        }
    }

    internal class ExceptionResponse
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public int ErrorCode { get; set; }
        public string Message { get; set; }
    }
}
