using System.Threading.Tasks;
using Rabbitr.Messages;
using Rabbitr.Responses;

namespace Rabbitr.Net.Abstractions
{
    public interface IRabbitrHandler
    {
         Task<RpcMessage> Handle(byte[] body);
    }
}