using System.Threading.Tasks;

namespace rabbitr.net.Abstractions
{
    public interface IRabbitrHandler
    {
         Task<IRabbitrResponse> Handle(byte[] body);
    }
}