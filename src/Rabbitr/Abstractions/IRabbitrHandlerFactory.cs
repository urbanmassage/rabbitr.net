using System.Threading.Tasks;
using Rabbitr.Responses;

namespace Rabbitr.Net.Abstractions
{
    public interface IRabbitrHandlerFactory
    {
        Task<Response> Handle<T1>(byte[] body);
    }
}