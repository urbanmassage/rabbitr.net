using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using Rabbitr.Net.Abstractions;
using Rabbitr.Messages;
using Rabbitr.Responses;
using Rabbitr.Utilities;

namespace Rabbitr.Net 
{
    public class RabbitrClient : IRabbitrClient
    {
        private readonly IRabbitrConnection _connection;
        private IRabbitrHandlerFactory _handlerFactory;
        private IRabbitrLogging _logging;
        private string RpcQueueName(string queueName) => $"rpc.{queueName}";
        private string ReturnQueueName(string queueName, string id) => $"{queueName}.return.{id}";
        private const int TimeoutSeconds = 10;

        public RabbitrClient(
            IRabbitrConnection connection, 
            IRabbitrHandlerFactory handlerFactory,
            IRabbitrLogging logging) 
        {
            _connection = connection;
            _handlerFactory = handlerFactory;
            _logging = logging;
        }

        public void Send(string exchangeName, RpcMessage message)
        {
            try
            {
                _connection.sendChannel.ExchangeDeclare(exchangeName, "topic", false, false, null);
                var jsonData = RpcMessages.ToByte(message);

                var properties = _connection.sendChannel.CreateBasicProperties();
                properties.ContentType = "application/json";
            
                _connection.sendChannel.BasicPublish(exchangeName, "*", false, properties, jsonData);
            }
            catch(Exception ex)
            {
                _logging.LogError($"ERROR - {ex.InnerException}");
            }
        }

        public void BindAndExchangeToQueue(string exchangeName, string queueName)
        {
            _connection.sendChannel.ExchangeDeclare(exchangeName, "topic", false, false, null);
            _connection.sendChannel.QueueDeclare(queueName, false, false, false, null);

            _connection.sendChannel.QueueBind(queueName, exchangeName, "*", null);
        }

        public void SubscribeToQueue<THandler>(string queueName) where THandler : IRabbitrHandler
        {
            var consumer = new EventingBasicConsumer(_connection.recieveChannel);
            _connection.recieveChannel.BasicConsume(queueName, false, "", false, false, null, consumer);

            consumer.Received += async (model, ea) => 
            {
                var body = ea.Body;
                var response = await _handlerFactory.Handle<THandler>(body);
                
                if(response is OkResponseGeneric<RpcMessage>)
                {

                    _connection.recieveChannel.BasicAck(ea.DeliveryTag, false);
                }
                else
                {
                    _connection.recieveChannel.BasicReject(ea.DeliveryTag, true);
                }
            };
        }

        public async Task<RpcMessage> RpcExec(string queueName, string message)
        {
            var taskCompletion = new TaskCompletionSource<RpcMessage>();

            var id = Guid.NewGuid().ToString();
            var returnQueue = ReturnQueueName(queueName, id);

            _connection.recieveChannel.QueueDeclare(returnQueue, false, false, true, null);

            var consumer = new EventingBasicConsumer(_connection.recieveChannel);
            _connection.recieveChannel.BasicConsume(returnQueue, false, "", false, true, null, consumer);

            var expiration = UnixTime.To(DateTime.Now.AddSeconds(TimeoutSeconds));
            var request = new RpcRequestMessage(message, returnQueue, expiration);
            
            var r = RpcMessages.ToByte(request);
            
            Thread.Sleep(TimeSpan.FromTicks(1));
            var rpcQueueName = RpcQueueName(queueName);

            var properties = _connection.sendChannel.CreateBasicProperties();
            properties.ContentType = "application/json";
            properties.Expiration = expiration.ToString();

            _connection.sendChannel.BasicPublish("", rpcQueueName, false, properties, r);

            consumer.Received += (model, ea) => 
            {
                _connection.recieveChannel.QueueDelete(returnQueue, false, true);
                if(DateTime.Now > (UnixTime.From(expiration)))
                {
                    //TODO: Error RPC timeout
                    taskCompletion.SetException(new Exception());
                }
                else
                {
                    var m = RpcMessages.FromByte(ea.Body);
                    _connection.recieveChannel.BasicAck(ea.DeliveryTag, false);
                    
                    taskCompletion.SetResult(m);
                }
            };

            return await taskCompletion.Task;
        }

        public void RpcListen<THandler>(string queueName)
        {
            var rpcQueueName = RpcQueueName(queueName);

            _connection.sendChannel.QueueDeclare(rpcQueueName, false, false, false, null);
            
            var consumer = new EventingBasicConsumer(_connection.recieveChannel);
            _connection.recieveChannel.BasicConsume(rpcQueueName, false, "", false, true, null, consumer);

            consumer.Received += async (model, ea) => 
            {
                var handlerResponse = await _handlerFactory.Handle<THandler>(ea.Body);

                if(handlerResponse is OkResponseGeneric<RpcMessage>)
                {
                    var hResponse = handlerResponse as OkResponseGeneric<RpcMessage>;

                    var message = new RpcMessage(hResponse.Data.Data);
                    var responseMessage = RpcMessages.ToByte(message);
                    
                    var properties = _connection.sendChannel.CreateBasicProperties();
                    properties.ContentType = "application/json";

                    _connection.sendChannel.BasicPublish("", hResponse.Data.ReturnQueue, false, properties, responseMessage);
                    _connection.recieveChannel.BasicAck(ea.DeliveryTag, false);
                }
                else
                {
                    //LOG ERROR
                }
            };
        }
    }
}