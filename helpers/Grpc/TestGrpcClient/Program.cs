using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using TestGrpc;

namespace TestGrpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddTransient<Service>();

            services.AddGrpcClient<Greeter.GreeterClient>();

            var serviceProvider = services.BuildServiceProvider();

            var test = serviceProvider.GetRequiredService<Service>();

            await test.SayHello("Hello");
        }
    }

    public class Service
    {
        private readonly Greeter.GreeterClient _client;

        public Service(Greeter.GreeterClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<string> SayHello(string name)
        {
            var response = await _client.SayHelloUnaryAsync(new HelloRequest { Name = name });

            return response.Message;
        }

        public async Task<string> StreamHellos(IEnumerable<string> names)
        {
            var call = _client.SayHelloClientStream();

            foreach (var name in names)
            {
                await call.RequestStream.WriteAsync(new HelloRequest { Name = name });
            }

            await call.RequestStream.CompleteAsync();

            var result = await call;

            return result.Message;
        }

        public async IAsyncEnumerable<string> ReceiveHelloStream(string name)
        {
            var call = _client.SayHelloServerStream(new HelloRequest { Name = name });

            await foreach (var item in call.ResponseStream.ReadAllAsync())
            {
                yield return item.Message;
            }
        }

        public async IAsyncEnumerable<string> HelloDuplex(IEnumerable<string> names)
        {
            var call = _client.SayHelloDuplex();

            foreach (var name in names)
            {
                await call.RequestStream.WriteAsync(new HelloRequest { Name = name });
            }

            await call.RequestStream.CompleteAsync();

            await foreach (var item in call.ResponseStream.ReadAllAsync())
            {
                yield return item.Message;
            }
        }
    }
}
