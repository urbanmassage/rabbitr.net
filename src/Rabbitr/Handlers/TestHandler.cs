using System.Threading.Tasks;
using rabbitr.net.Abstractions;

namespace Rabbitr.Handlers
{
    public class TestHandler : IRabbitrHandler
    {
        public Task<IRabbitrResponse> Handle(byte[] body)
        {
            throw new System.NotImplementedException();
        }
    }
}