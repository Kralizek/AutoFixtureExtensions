using System.Linq;
using AutoFixture;
using Kralizek.AutoFixture.Extensions.Internal;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using Greeter = TestGrpc.Greeter;

namespace Tests
{
    public class GrpcServerFixtureExtensionsTests
    {
        [Test, CustomAutoData]
        public void AddGrpcServerSupport_registers_customization(IFixture fixture)
        {
            GrpcServerFixtureExtensions.AddGrpcServerSupport<Greeter.GreeterClient, TestGrpcService.Startup>(fixture, b => b.UseSolutionRelativeContentRoot("helpers"));

            Assert.Multiple(() =>
            {
                Assert.That(() => fixture.Create<WebApplicationFactory<TestGrpcService.Startup>>(), Throws.Nothing);

                Assert.That(fixture.Customizations, Has.Exactly(1).InstanceOf<GrpcClientSpecimenBuilder<Greeter.GreeterClient, TestGrpcService.Startup>>());
            });
        }
    }
}