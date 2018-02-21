using RabbitMQ.Client;

namespace Rabbitr.Net.Abstractions
{
    public interface IRabbitrConnection
    {
        IModel sendChannel { get; }
        IModel recieveChannel { get; }
    }
}