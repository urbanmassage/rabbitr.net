using System.Collections.Concurrent;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using rabbitr.net.Abstractions;
using rabbitr.net.Responses;

namespace rabbitr.net 
{
    public class Rabbitr 
    {
        private readonly IRabbitrConnection connection;

        private readonly BlockingCollection<string> responseQueue = new BlockingCollection<string>();

        public Rabbitr(IRabbitrConnection connection) 
        {
            this.connection = connection;
        }

        public void Send<T>(string exchangeName, T message) where T : IRabbitrMessage
        {
            connection.sendChannel.ExchangeDeclare(exchangeName, "topic", false, false, null);
            var jsonData = ToByte(message);

            var properties = connection.sendChannel.CreateBasicProperties();
            properties.ContentType = "application/json";
        
            connection.sendChannel.BasicPublish(exchangeName, "*", false, properties, jsonData);
        }

        public void BindAndExchangeToQueue(string exchangeName, string queueName)
        {
            connection.sendChannel.ExchangeDeclare(exchangeName, "topic", false, false, null);
            connection.sendChannel.QueueDeclare(queueName, false, false, false, null);

            connection.sendChannel.QueueBind(queueName, exchangeName, "*", null);
        }

        public void SubscribeToQueue<T>(string queueName, T handler) where T : IRabbitrHandler
        {
            var consumer = new EventingBasicConsumer(connection.recieveChannel);
            connection.recieveChannel.BasicConsume(queueName, false, "", false, false, null, consumer);

            consumer.Received += async (model, ea) => 
            {
                var body = ea.Body;
                var response = await handler.Handle(body);

                if(response is RabbitrErrorResponse)
                {
                    connection.recieveChannel.BasicNack(ea.DeliveryTag, false, true);
                }
                else
                {
                    connection.recieveChannel.BasicAck(ea.DeliveryTag, false);
                }
            };
        }

        private byte[] ToByte<T>(T message) 
        {
            var messageString = JsonConvert.SerializeObject(message);
            return Encoding.UTF8.GetBytes(messageString);
        }

        private T FromByte<T>(byte[] message)
        {
            var messageString = Encoding.UTF8.GetString(message);
            return JsonConvert.DeserializeObject<T>(messageString);
        }
    }
}