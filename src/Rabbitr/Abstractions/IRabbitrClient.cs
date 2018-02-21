using System.Threading.Tasks;
using Rabbitr.Messages;

namespace rabbitr.net.Abstractions
{
    public interface IRabbitrClient
    {
        void Send(string exchangeName, RpcMessage message);
        
        void BindAndExchangeToQueue(string exchangeName, string queueName);

        void SubscribeToQueue<THandler>(string queueName) where THandler : IRabbitrHandler;

        Task<RpcMessage> RpcExec(string queueName, string message);

        void RpcListen<THandler>(string queueName);
    }
}