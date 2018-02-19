using Microsoft.Extensions.Options;
using Moq;
using rabbitr.net.Configuration;
using Rabbitr.Handlers;
using Xunit;

namespace rabbitr.net.Tests
{
    public class Rabbitr_Tests
    {
        [Fact]
        public void Test()
        {
            var configuration = new Mock<IOptions<RabbitrConfiguration>>();
            configuration.Setup(x => x.Value).Returns(new RabbitrConfiguration{ HostName = "localhost", PortNumber = 32771 });

            var rabbitrConnection = new RabbitrConnection(configuration.Object);
            var rabbitr = new Rabbitr(rabbitrConnection);

            var handler = new TestHandler();
            rabbitr.SubscribeToQueue("TestQueue", handler);
        }
    }
}