using System;
using System.Text;
using Newtonsoft.Json;
using Rabbitr.Exceptions;
using Rabbitr.Messages;

namespace Rabbitr.Utilities
{
    public static class RpcMessages
    {
        public static RpcMessage FromByte(byte[] message)
        {
            try
            {
                var messageString = Encoding.UTF8.GetString(message);
                return JsonConvert.DeserializeObject<RpcMessage>(messageString);
            }
            catch(Exception ex)
            {
                throw new MessageConversionException("An error occured whilst converting bytes to message", ex);
            }
        }

        public static byte[] ToByte<T>(T message) 
        {
            try
            {
                var messageString = JsonConvert.SerializeObject(message);
                return Encoding.UTF8.GetBytes(messageString);
            }
            catch(Exception ex)
            {
                throw new MessageConversionException("An error occured whilst converting message to bytes", ex);
            }            
        }
    }
}