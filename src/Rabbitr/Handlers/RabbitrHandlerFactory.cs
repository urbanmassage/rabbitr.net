using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rabbitr.net;
using rabbitr.net.Abstractions;
using Rabbitr.Exceptions;
using Rabbitr.Messages;
using Rabbitr.Responses;

namespace Rabbitr.Handlers
{
    public class RabbitrHandlerFactory : IRabbitrHandlerFactory
    {
        private readonly IEnumerable<IRabbitrHandler> _handlers;

        public RabbitrHandlerFactory(IEnumerable<IRabbitrHandler> handlers)
        {
            this._handlers = handlers;
        }

        public async Task<Response> Handle<THandler>(byte[] body)
        {
            try
            {
                var handler = (IRabbitrHandler)_handlers.OfType<THandler>().First();
                var response = await handler.Handle(body);
                return new OkResponseGeneric<RpcMessage>(response);
            }
            catch(HandlerException hex)
            {
                return new ErrorResponse(new Error("Handler Error", hex.InnerException.ToString()));
            }
            catch(Exception ex)
            {
                return new ErrorResponse(new Error("Generic Error", ex.InnerException.ToString()));
            }
        }
    }
}