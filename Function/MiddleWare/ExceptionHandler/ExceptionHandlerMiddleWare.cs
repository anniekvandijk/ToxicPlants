using Function.UseCases;
using Microsoft.Azure.Functions.Worker;
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

                if (ex.InnerException != null && ex.InnerException.GetType() == typeof(ProgramError))
                {
                    await HandleResponse.SetProgramErrorResponse(context, logger, ex.InnerException);
                }
                else
                {
                    await HandleResponse.SetExceptionResponse(context, logger, HttpStatusCode.InternalServerError, ex);
                }
            }
        }
    }
}
