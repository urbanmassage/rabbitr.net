using System.Threading.Tasks;
using Rabbitr.Messages;
using Rabbitr.Responses;

namespace rabbitr.net.Abstractions
{
    public interface IRabbitrHandler
    {
         Task<RpcMessage> Handle(byte[] body);
    }
}