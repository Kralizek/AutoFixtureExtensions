using System;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using TestGrpc;

namespace Tests.Integration
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TestAutoDataAttribute : AutoDataAttribute
    {
        public TestAutoDataAttribute() : base(() => new Fixture().AddGrpcServerSupport<Greeter.GreeterClient, TestGrpcService.Startup>(b => b.UseSolutionRelativeContentRoot("helpers"))) { }
    }

    [TestFixture]
    public class IntegrationTests
    {
        [Test]
        public async Task Should_work_with_no_auto_data_attribute()
        {
            var fixture = new Fixture().AddGrpcServerSupport<Greeter.GreeterClient, TestGrpcService.Startup>(b => b.UseSolutionRelativeContentRoot("helpers"));

            var name = fixture.Create<string>();

            var client = fixture.Create<Greeter.GreeterClient>();

            var response = await client.SayHelloUnaryAsync(new HelloRequest { Name = name });

            Assert.That(response.Message, Is.EqualTo(GreeterService.FormatMessage(name)));
        }

        [Test, AutoData]
        public async Task Should_work_with_basic_auto_data_attribute(IFixture fixture, string name)
        {
            fixture.AddGrpcServerSupport<Greeter.GreeterClient, TestGrpcService.Startup>(b => b.UseSolutionRelativeContentRoot("helpers"));

            var client = fixture.Create<Greeter.GreeterClient>();

            var response = await client.SayHelloUnaryAsync(new HelloRequest { Name = name });

            Assert.That(response.Message, Is.EqualTo(GreeterService.FormatMessage(name)));
        }

        [Test, TestAutoData]
        public async Task Should_work_with_custom_auto_data_attribute(Greeter.GreeterClient client, string name)
        {
            var response = await client.SayHelloUnaryAsync(new HelloRequest { Name = name });

            Assert.That(response.Message, Is.EqualTo(GreeterService.FormatMessage(name)));
        }
    }
}
