using System;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using rabbitr.net.Abstractions;

namespace testing_console
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync()
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
                            .AddEnvironmentVariables();

            var configuration = builder.Build();
            var serviceCollection = new ServiceCollection();
            var container = AutofacContainerBuilder.Build(serviceCollection, configuration);

            var rabbitr = container.Resolve<IRabbitrClient>();

            rabbitr.RpcListen<NewMessageHandler>("TestQueue");
            var response = await rabbitr.RpcExec("TestQueue", "this is a message");


            Console.WriteLine(response.Data);
            Console.ReadLine();
        }
    }
}
