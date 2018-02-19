using RabbitMQ.Client;

namespace rabbitr.net.Abstractions
{
    public interface IRabbitrConnection
    {
        IModel sendChannel { get; }
        IModel recieveChannel { get; }
    }
}