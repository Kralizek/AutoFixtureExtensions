using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using Kralizek.AutoFixture.Extensions.Internal;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using TestGrpc;

namespace Tests.Internal
{
    [TestFixture]
    public class GrpcClientSpecimenBuilderTests
    {
        [Test, CustomAutoData]
        public void Constructor_is_guarded(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(GrpcClientSpecimenBuilder<Greeter.GreeterClient, TestGrpcService.Startup>).GetConstructors());
        }

        [Test, CustomAutoData]
        public void RequestSpecification_exposes_passed_specification(IRequestSpecification requestSpecification)
        {
            var sut = new GrpcClientSpecimenBuilder<Greeter.GreeterClient, TestGrpcService.Startup>(requestSpecification);

            Assert.That(sut.RequestSpecification, Is.SameAs(requestSpecification));
        }

        [Test, CustomAutoData]
        public void Default_RequestSpecification_is_HttpClientRequestSpecification()
        {
            var sut = new GrpcClientSpecimenBuilder<Greeter.GreeterClient, TestGrpcService.Startup>();

            Assert.That(sut.RequestSpecification, Is.InstanceOf<GrpcClientSpecimenBuilder<Greeter.GreeterClient, TestGrpcService.Startup>.GrpcClientRequestSpecification>());
        }

        [Test, CustomAutoData]
        public void Create_is_guarded(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(GrpcClientSpecimenBuilder<Greeter.GreeterClient, TestGrpcService.Startup>).GetMethod(nameof(GrpcClientSpecimenBuilder<Greeter.GreeterClient, TestGrpcService.Startup>.Create)));
        }

        [Test, CustomAutoData]
        public void Create_returns_HttpClient_if_requested(GrpcClientSpecimenBuilder<Greeter.GreeterClient, TestGrpcService.Startup> sut, IFixture fixture)
        {
            fixture.Inject(new WebApplicationFactory<TestGrpcService.Startup>().WithWebHostBuilder(b => b.UseSolutionRelativeContentRoot("")));

            var result = sut.Create(typeof(Greeter.GreeterClient), fixture.Create<SpecimenContext>());

            Assert.That(result, Is.InstanceOf<Greeter.GreeterClient>());
        }

        [Test, CustomAutoData]
        public void Create_returns_NoSpecimen_if_request_is_invalid(GrpcClientSpecimenBuilder<Greeter.GreeterClient, TestGrpcService.Startup> sut, SpecimenContext context)
        {
            var result = sut.Create(typeof(object), context);

            Assert.That(result, Is.InstanceOf<NoSpecimen>());
        }
    }
}
