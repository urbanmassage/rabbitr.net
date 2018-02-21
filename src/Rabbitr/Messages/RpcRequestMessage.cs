namespace Rabbitr.Messages
{
    public class RpcRequestMessage : RpcMessage
    {   
        public RpcRequestMessage(string data, string returnQueue, int expiration)
            : base(data, returnQueue, expiration)
        {
        }
    }
}