using System;
using Autofac;
using Rabbitr.Handlers;
using Rabbitr.Logging;
using Rabbitr.Net;
using Rabbitr.Net.Abstractions;

namespace Rabbitr.Autofac
{
    public class RabbitrRegistrations : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RabbitrConnection>().As<IRabbitrConnection>();
            builder.RegisterType<NullLogger>().As<IRabbitrLogging>();
            builder.RegisterType<RabbitrHandlerFactory>().As<IRabbitrHandlerFactory>();
            builder.RegisterType<RabbitrClient>().As<IRabbitrClient>().SingleInstance();
        }
    }
}
