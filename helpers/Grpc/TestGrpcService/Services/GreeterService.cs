using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace TestGrpc
{
    public class GreeterService : TestGrpc.Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public static string FormatMessage(string name) => $"Hello {name}";

        public override Task<HelloReply> SayHelloUnary(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = FormatMessage(request.Name)
            });
        }

        public override async Task SayHelloDuplex(IAsyncStreamReader<HelloRequest> requestStream, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
        {
            await foreach (var item in requestStream.ReadAllAsync())
            {
                await responseStream.WriteAsync(new HelloReply { Message = FormatMessage(item.Name) });
            }
        }

        public override async Task SayHelloServerStream(HelloRequest request, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
        {
            for (var i = 0; i < 10; i++)
            {
                await responseStream.WriteAsync(new HelloReply { Message = FormatMessage(request.Name) });
            }
        }

        public override async Task<HelloReply> SayHelloClientStream(IAsyncStreamReader<HelloRequest> requestStream, ServerCallContext context)
        {
            var names = new List<string>();

            await foreach (var item in requestStream.ReadAllAsync())
            {
                names.Add(item.Name);
            }

            return new HelloReply { Message = FormatMessage(string.Join(", ", names)) };
        }
    }
}
