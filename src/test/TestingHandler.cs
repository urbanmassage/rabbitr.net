using System;
using System.Threading.Tasks;
using rabbitr.net;
using rabbitr.net.Abstractions;
using Rabbitr.Messages;
using Rabbitr.Responses;
using Rabbitr.Utilities;

namespace test
{
    public class TestingHandler : IRabbitrHandler
    {
        public RpcMessage Response {get; private set;}

        public Task<RpcMessage> Handle(byte[] body)
        {
            Response = Messages.FromByte(body);
            return Task.FromResult(Response);
        }
    }
}