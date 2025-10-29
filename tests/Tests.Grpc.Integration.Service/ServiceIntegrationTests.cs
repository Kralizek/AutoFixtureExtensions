using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;
using Grpc.Core;
using Moq;
using NUnit.Framework;
using TestGrpc;

namespace Tests
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ServiceAutoDataAttribute : AutoDataAttribute
    {
        public ServiceAutoDataAttribute() : base(CreateFixture) { }

        private static IFixture CreateFixture()
        {
            var fixture = new Fixture();

            fixture.AddGrpcSupport();

            fixture.Customize(new AutoMoqCustomization
            {
                ConfigureMembers = true,
                GenerateDelegates = true
            });

            return fixture;
        }
    }

    [TestFixture]
    public class ServiceIntegrationTests
    {
        [Test, ServiceAutoData]
        public async Task Service_greets_sender_unary(GreeterService sut, string name, ServerCallContext context)
        {
            var response = await sut.SayHelloUnary(new HelloRequest { Name = name }, context);

            Assert.That(response.Message, Does.EndWith(name));
        }

        [Test, ServiceAutoData]
        public async Task Service_greets_sender_client_stream(GreeterService sut, ServerCallContext context, [Frozen] IEnumerable<HelloRequest> requests, IAsyncStreamReader<HelloRequest> requestStream)
        {
            var response = await sut.SayHelloClientStream(requestStream, context);

            foreach (var req in requests)
            {
                Assert.That(response.Message, Does.Contain(req.Name));
            }
        }

        [Test, ServiceAutoData]
        public async Task Service_greets_sender_server_stream(GreeterService sut, ServerCallContext context, IServerStreamWriter<HelloReply> responseStream, string name)
        {
            await sut.SayHelloServerStream(new HelloRequest { Name = name }, responseStream, context);

            Mock.Get(responseStream).Verify(p => p.WriteAsync(It.Is<HelloReply>(r => r.Message.EndsWith(name))), Times.Exactly(10));
        }

        [Test, ServiceAutoData]
        public async Task Service_greets_sender_duplex(GreeterService sut, ServerCallContext context, [Frozen] IEnumerable<HelloRequest> requests, IAsyncStreamReader<HelloRequest> requestStream, IServerStreamWriter<HelloReply> responseStream)
        {
            await sut.SayHelloDuplex(requestStream, responseStream, context);

            foreach (var req in requests)
            {
                Mock.Get(responseStream).Verify(p => p.WriteAsync(It.Is<HelloReply>(r => r.Message.EndsWith(req.Name))), Times.Once());
            }
        }

        [Test, ServiceAutoData]
        public async Task Service_greets_many_senders(GreeterService sut, ServerCallContext context, ManyHellosRequest request)
        {
            var response = await sut.SayHelloMany(request, context);

            foreach (var name in request.Names)
            {
                Assert.That(response.Messages, Does.Contain($"Hello {name}"));
            }
        }
    }
}