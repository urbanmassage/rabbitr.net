using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rabbitr.Autofac;
using Rabbitr.Handlers;
using Rabbitr.Logging;

namespace testing_console
{
    public class AutofacContainerBuilder
    {
        public static IContainer Build(IServiceCollection collection, IConfigurationRoot configuration)
        {
            SetupOptions(collection, configuration);

            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(collection);
            containerBuilder.RegisterModule<RabbitrRegistrations>();
            containerBuilder.RegisterType<NewMessageHandler>().As<IRabbitrHandler>();
            return containerBuilder.Build();
        }

         private static void SetupOptions(
            IServiceCollection collection, 
            IConfigurationRoot configuration)
        {
            collection.AddOptions();
            collection.Configure<RabbitrConfiguration>(configuration.GetSection("RabbitMq"));
        }
    }
}