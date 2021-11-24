using System;
using System.Net;

namespace Function.MiddleWare.ExceptionHandler
{
    internal class ProgramError : Exception
    {
        public ProgramError(string message)
            : base(message)
        {
        }

        public ProgramError(string message, Exception inner)
            : base(message, inner)
        {
        }

        public static void CreateProgramError(HttpStatusCode code, string message, int errorCode = 0)
        {
            var ex = new ProgramError(message);
            ex.Data.Add("statusCode", (int)code);
            ex.Data.Add("errorCode", errorCode);
            throw ex;
        }

        public static void CreateProgramError(HttpStatusCode code, string message, Exception exception, int errorCode = 0)
        {
            var ex = new ProgramError(message, exception);
            ex.Data.Add("statusCode", (int)code);
            ex.Data.Add("errorCode", errorCode);
            throw ex;
        }
    }
}
