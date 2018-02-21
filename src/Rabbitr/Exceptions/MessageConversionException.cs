using System;
using System.Runtime.Serialization;

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

        protected MessageConversionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}