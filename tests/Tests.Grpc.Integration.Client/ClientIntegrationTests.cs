using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;
using Grpc.Core;
using Moq;
using NUnit.Framework;
using TestGrpc;
using TestGrpcClient;

namespace Tests
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ClientAutoDataAttribute : AutoDataAttribute
    {
        public ClientAutoDataAttribute() : base(CreateFixture) { }

        private static IFixture CreateFixture()
        {
            var fixture = new Fixture();

            fixture.AddGrpcSupport();

            fixture.Customize(new AutoMoqCustomization
            {
                ConfigureMembers = true,
                GenerateDelegates = true
            });

            fixture.Register(Mock.Of<Greeter.GreeterClient>);

            return fixture;
        }
    }

    [TestFixture]
    public class ClientIntegrationTests
    {
        [Test, ClientAutoData]
        public async Task Service_uses_internal_client_for_unary_requests([Frozen] Greeter.GreeterClient client, Service sut, [Frozen] HelloReply reply, AsyncUnaryCall<HelloReply> responseCall, string name)
        {
            Mock.Get(client).Setup(p => p.SayHelloUnaryAsync(It.IsAny<HelloRequest>(), null, null, default)).Returns(responseCall);

            var response = await sut.SayHello(name);

            Assert.That(response, Is.EqualTo(reply.Message));
        }

        [Test, ClientAutoData]
        public async Task Service_uses_internal_client_for_client_streaming_requests([Frozen] Greeter.GreeterClient client, Service sut, [Frozen] HelloReply reply, AsyncClientStreamingCall<HelloRequest, HelloReply> responseCall, string[] names)
        {
            Mock.Get(client).Setup(p => p.SayHelloClientStream(null, null, default)).Returns(responseCall);

            var response = await sut.StreamHellos(names);

            Assert.That(response, Is.EqualTo(reply.Message));
        }

        [Test, ClientAutoData]
        public async Task Service_uses_internal_client_for_server_streaming([Frozen] Greeter.GreeterClient client, Service sut, [Frozen] HelloReply reply, AsyncServerStreamingCall<HelloReply> responseCall, string name)
        {
            Mock.Get(client).Setup(p => p.SayHelloServerStream(It.IsAny<HelloRequest>(), null, null, default)).Returns(responseCall);

            var response = await sut.ReceiveHelloStream(name).ToListAsync();

            Assert.That(response, Contains.Item(reply.Message));
        }

        [Test, ClientAutoData]
        public async Task Service_uses_internal_client_for_duplex_streaming([Frozen] Greeter.GreeterClient client, Service sut, [Frozen] HelloReply reply, AsyncDuplexStreamingCall<HelloRequest, HelloReply> responseCall, string[] names)
        {
            Mock.Get(client).Setup(p => p.SayHelloDuplex(null, null, default)).Returns(responseCall);

            var response = await sut.HelloDuplex(names).ToListAsync();

            Assert.That(response, Contains.Item(reply.Message));
        }
    }
}
