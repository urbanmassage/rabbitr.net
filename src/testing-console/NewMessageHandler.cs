using System.Threading.Tasks;
using rabbitr.net.Abstractions;
using Rabbitr.Messages;
using Rabbitr.Utilities;

namespace testing_console
{
    public class NewMessageHandler : IRabbitrHandler
    {
        public Task<RpcMessage> Handle(byte[] body)
        {
            var message = Messages.FromByte(body);
            return Task.FromResult(message);
        }
    }
}