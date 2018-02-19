using System;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using rabbitr.net.Abstractions;
using rabbitr.net.Configuration;

namespace rabbitr.net
{
    public class RabbitrConnection : IRabbitrConnection
    {
        // public readonly ConnectionFactory factory;
        public IModel sendChannel { get; }
        public IModel recieveChannel { get; }

        public RabbitrConnection(IOptions<RabbitrConfiguration> config)
        {
            var factory = new ConnectionFactory { HostName = config.Value.HostName, Port = config.Value.PortNumber };
            var connection = factory.CreateConnection();
            sendChannel = connection.CreateModel();
            recieveChannel = connection.CreateModel();
        }
    }
}
