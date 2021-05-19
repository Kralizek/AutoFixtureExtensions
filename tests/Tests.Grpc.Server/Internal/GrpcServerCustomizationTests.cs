using AutoFixture;
using AutoFixture.Idioms;
using Kralizek.AutoFixture.Extensions.Internal;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using TestGrpc;

namespace Tests.Internal
{
    [TestFixture]
    public class GrpcServerCustomizationTests
    {
        [Test, CustomAutoData]
        public void Constructor_does_not_accept_nulls(GuardClauseAssertion assertion) => assertion.Verify(typeof(GrpcServerCustomization<Greeter.GreeterClient, TestGrpcService.Startup>));

        [Test, CustomAutoData]
        public void Customize_injects_instance_of_WebApplicationFactory(GrpcServerCustomization<Greeter.GreeterClient, TestGrpcService.Startup> sut, IFixture fixture)
        {
            sut.Customize(fixture);

            Assert.That(() => fixture.Create<WebApplicationFactory<TestGrpcService.Startup>>(), Throws.Nothing);
        }

        [Test, CustomAutoData]
        public void Multiple_requests_for_WebApplicationFactory_return_same_instance(GrpcServerCustomization<Greeter.GreeterClient, TestGrpcService.Startup> sut, IFixture fixture)
        {
            sut.Customize(fixture);

            var first = fixture.Create<WebApplicationFactory<TestGrpcService.Startup>>();

            var second = fixture.Create<WebApplicationFactory<TestGrpcService.Startup>>();

            Assert.That(first, Is.SameAs(second));
        }

        [Test, CustomAutoData]
        public void Customize_registers_GrpcClientSpecimenBuilder(GrpcServerCustomization<Greeter.GreeterClient, TestGrpcService.Startup> sut, IFixture fixture)
        {
            sut.Customize(fixture);

            Assert.That(fixture.Customizations, Has.Exactly(1).InstanceOf<GrpcClientSpecimenBuilder<Greeter.GreeterClient, TestGrpcService.Startup>>());
        }
    }
}
