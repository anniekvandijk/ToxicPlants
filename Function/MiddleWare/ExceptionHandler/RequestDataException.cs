using System;

namespace Function.MiddleWare.ExceptionHandler
{
    public class RequestDataException : Exception
    {
        public RequestDataException(string message)
            : base(message)
        {
        }

        public RequestDataException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
