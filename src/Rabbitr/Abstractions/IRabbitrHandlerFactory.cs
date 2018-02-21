using System.Threading.Tasks;
using Rabbitr.Responses;

namespace rabbitr.net.Abstractions
{
    public interface IRabbitrHandlerFactory
    {
        Task<Response> Handle<T1>(byte[] body);
    }
}