using System;
using System.Threading;
using Autofac;
using Microsoft.Extensions.Options;
using Moq;
using rabbitr.net.Abstractions;
using rabbitr.net.Configuration;
using Rabbitr.Handlers;
using Rabbitr.Messages;
using Shouldly;
using test;
using TestStack.BDDfy;
using Xunit;

namespace rabbitr.net.Tests
{
    public class Rabbitr_Tests
    {
        private IContainer _container;
        private RabbitrClient _client;
        private string _queueName;
        private string _exchangeName;
        private string _messageText;
        private TestingHandler _handler;
        private RpcMessage _response;

        [Fact]
        public void WhenSendingAMessageTheHandlerRecievesTheCorrectInformation()
        {
            this.Given(_ => _.TheApplicationIsRegistered())
                    .And(_ => _.ThereIsARabbitrClient())
                    .And(_ => _.TheApplicationIsBoundToAQueue())
                    .And(_ => _.TheApplicationIsSubscribingToTheQueue())
                .When(_ => _.AMessageIsSent())
                    .And(_ => _.WeHaveWaitedForTheMessageToBeRead())
                .Then(_ => _.TheRecievedMessageShouldBeTheCorrectOne())
                .BDDfy();
        }

        [Fact]
        public void WhenSendingAnRpcMessageTheCorrectOneIsReturned()
        {
            this.Given(_ => _.TheApplicationIsRegistered())
                    .And(_ => _.ThereIsARabbitrClient())
                    .And(_ => _.TheApplicationIsBoundToAQueue())
                    .And(_ => _.TheApplicationIsSubscribingToTheQueue())
                    .And(_ => _.ThereIsAListener())
                .When(_ => _.AnRpcMessageIsSent())
                .Then(_ => _.TheRecievedMessageShouldBeTheCorrectOne())
                .BDDfy();
        }

        private void ThereIsAListener()
        {
            _client.RpcListen<TestingHandler>(_queueName);
        }

        private void AnRpcMessageIsSent()
        {
            _messageText = Guid.NewGuid().ToString();
            _response = _client.RpcExec(_queueName, _messageText).Result;
        }

        private void TheMessageInTheResponseShouldBeCorrect()
        {
            _response.Data.ShouldBe(_messageText);
        }

        private void TheRecievedMessageShouldBeTheCorrectOne()
        {
            var stringMessage = _handler.Response.Data;
            stringMessage.ShouldBe(_messageText);
        }

        private void WeHaveWaitedForTheMessageToBeRead()
        {
            Thread.Sleep(10000);
        }
        private void AMessageIsSent()
        {
            _messageText = Guid.NewGuid().ToString();
            _client.Send(_exchangeName, new RpcMessage(_messageText, string.Empty, 0));
        }

        private void TheApplicationIsSubscribingToTheQueue()
        {
            _client.SubscribeToQueue<TestingHandler>(_queueName);
        }

        private void TheApplicationIsBoundToAQueue()
        {
            _queueName = Guid.NewGuid().ToString();
            _exchangeName = Guid.NewGuid().ToString();
            _client.BindAndExchangeToQueue(_exchangeName, _queueName);
        }

        private void TheApplicationIsRegistered()
        {
            _handler = new TestingHandler();
            _container = Registrations.Generate(_handler);
        }

        private void ThereIsARabbitrClient()
        {
            _client = _container.Resolve<RabbitrClient>();
        }
    }
}