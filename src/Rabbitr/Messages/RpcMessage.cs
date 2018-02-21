using rabbitr.net.Abstractions;

namespace Rabbitr.Messages
{
    public class RpcMessage : IRabbitrMessage
    {        
        public RpcMessage(string data, string returnQueue = "", int? expiration = null)
        {
            Data = data;
            ReturnQueue = returnQueue;
            Expiration = expiration;
        }

        public string Data {get;}
        public string ReturnQueue {get;}
        public int? Expiration {get;} 
    }
}