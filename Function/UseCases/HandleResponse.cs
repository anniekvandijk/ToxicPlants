using Function.MiddleWare.ExceptionHandler;
using Function.Models.Response;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Function.UseCases
{
    internal class HandleResponse
    {
        public static async Task<HttpResponseData> SetResponse(HttpRequestData request, string resultBody)
        {
            var response = request.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            await response.WriteStringAsync(resultBody);

            return response;
        }

        public static async Task SetExceptionResponse(FunctionContext context, ILogger<ExceptionHandlerMiddleware> logger, HttpStatusCode statusCode, Exception ex)
        {
            var body = new ErrorResponse
            {
                HttpStatusCode = statusCode,
                Message = ex.Message,
                ErrorCode = 999
            };

            var request = context.GetHttpRequestData(logger);
            var response = request.CreateResponse(statusCode);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            await response.WriteStringAsync(JsonSerializer.Serialize(body));
            context.SetHttpResponseData(response, logger);
        }

        public static async Task SetProgramErrorResponse(FunctionContext context, ILogger<ExceptionHandlerMiddleware> logger, Exception ex)
        {
            var statusCode = (HttpStatusCode)ex.Data["statusCode"];
            var errorCode = (int)ex.Data["errorCode"];
            var message = ex.Message;

            var body = new ErrorResponse
            {
                HttpStatusCode = statusCode,
                Message = message,
                ErrorCode = errorCode
            };

            var request = context.GetHttpRequestData(logger);
            var response = request.CreateResponse(statusCode);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            await response.WriteStringAsync(JsonSerializer.Serialize(body));
            context.SetHttpResponseData(response, logger);
        }
    }
}
