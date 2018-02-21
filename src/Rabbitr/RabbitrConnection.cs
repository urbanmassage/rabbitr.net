using System;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Rabbitr.Net.Abstractions;
using Rabbitr.Net.Configuration;

namespace Rabbitr.Net
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
