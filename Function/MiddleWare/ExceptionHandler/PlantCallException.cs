using System;

namespace Function.MiddleWare.ExceptionHandler
{
    internal class PlantCallException : Exception
    {
        public PlantCallException(string message)
            : base(message)
        {
        }

        public PlantCallException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
