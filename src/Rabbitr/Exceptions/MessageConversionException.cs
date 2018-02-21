using System;

namespace Rabbitr.Exceptions
{
    public class MessageConversionException : Exception
    {
        public MessageConversionException()
        {
        }

        public MessageConversionException(string message) : base(message)
        {
        }

        public MessageConversionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}