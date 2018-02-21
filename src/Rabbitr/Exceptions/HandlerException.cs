using System;

namespace Rabbitr.Exceptions
{
    public class HandlerException : Exception
    {
        public HandlerException()
        {
        }

        public HandlerException(string message) : base(message)
        {
        }

        public HandlerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}