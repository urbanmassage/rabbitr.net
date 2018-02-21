using System;
using System.Threading.Tasks;
using Rabbitr.Messages;
using Rabbitr.Net.Abstractions;
using Rabbitr.Responses;
using Rabbitr.Utilities;

namespace test
{
    public class TestingHandler : IRabbitrHandler
    {
        public RpcMessage Response {get; private set;}

        public Task<RpcMessage> Handle(byte[] body)
        {
            Response = RpcMessages.FromByte(body);
            return Task.FromResult(Response);
        }
    }
}