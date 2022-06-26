using Function.Interfaces;
using Function.MiddleWare.ExceptionHandler;
using Function.Models;
using Function.Models.Response;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Function.UseCases
{
    internal class HandleResponse : IHandleResponse
    {
        public async Task<HttpResponseData> SetResponse(HttpRequestData request, List<PlantResponse> result)
        {
            var json = JsonSerializer.Serialize(result);
            return await Createresponse(request, HttpStatusCode.OK, json);
        }

        public static async Task SetExceptionResponse(FunctionContext context, ILogger<ExceptionHandlerMiddleware> logger, HttpStatusCode statusCode, Exception ex)
        {
            var body = new ErrorResponse
            {
                HttpStatusCode = statusCode,
                Message = ex.Message,
                ErrorCode = 999
            };

            var serialize = JsonSerializer.Serialize(body);

            var request = context.GetHttpRequestData(logger);
            var response = await Createresponse(request, statusCode, serialize);
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

            var serialize = JsonSerializer.Serialize(body);

            var request = context.GetHttpRequestData(logger);
            var response = await Createresponse(request, statusCode, serialize);
            context.SetHttpResponseData(response, logger);
        }

        private static async Task<HttpResponseData> Createresponse(HttpRequestData request, HttpStatusCode statusCode, string body)
        {
            var response = request.CreateResponse(statusCode);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            await response.WriteStringAsync(body);
            return response;
        }
    }
}
