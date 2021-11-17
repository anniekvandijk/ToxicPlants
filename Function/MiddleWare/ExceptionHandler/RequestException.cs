using System;

namespace Function.MiddleWare.ExceptionHandler
{
    internal class RequestException : Exception
    {
        public RequestException(string message)
            : base(message)
        {
        }

        public RequestException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
