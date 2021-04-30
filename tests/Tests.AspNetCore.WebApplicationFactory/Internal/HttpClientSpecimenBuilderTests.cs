using System.Net.Http;
using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using Kralizek.AutoFixture.Extensions.Internal;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;

namespace Tests.Internal
{
    [TestFixture]
    public class HttpClientSpecimenBuilderTests
    {
        [Test, CustomAutoData]
        public void Constructor_is_guarded(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(HttpClientSpecimenBuilder<TestWebSite.Startup>).GetConstructors());
        }

        [Test, CustomAutoData]
        public void RequestSpecification_exposes_passed_specification(IRequestSpecification requestSpecification)
        {
            var sut = new HttpClientSpecimenBuilder<TestWebSite.Startup>(requestSpecification);

            Assert.That(sut.RequestSpecification, Is.SameAs(requestSpecification));
        }

        [Test, CustomAutoData]
        public void Default_RequestSpecification_is_HttpClientRequestSpecification()
        {
            var sut = new HttpClientSpecimenBuilder<TestWebSite.Startup>();

            Assert.That(sut.RequestSpecification, Is.InstanceOf<HttpClientRequestSpecification>());
        }

        [Test, CustomAutoData]
        public void Create_is_guarded(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(HttpClientSpecimenBuilder<TestWebSite.Startup>).GetMethod(nameof(HttpClientSpecimenBuilder<TestWebSite.Startup>.Create)));
        }

        [Test, CustomAutoData]
        public void Create_returns_HttpClient_if_requested(HttpClientSpecimenBuilder<TestWebSite.Startup> sut, IFixture fixture)
        {
            fixture.Inject(new WebApplicationFactory<TestWebSite.Startup>().WithWebHostBuilder(b => b.UseSolutionRelativeContentRoot("")));

            var result = sut.Create(typeof(HttpClient), fixture.Create<SpecimenContext>());

            Assert.That(result, Is.InstanceOf<HttpClient>());
        }

        [Test, CustomAutoData]
        public void Create_returns_NoSpecimen_if_request_is_invalid(HttpClientSpecimenBuilder<TestWebSite.Startup> sut, SpecimenContext context)
        {
            var result = sut.Create(typeof(object), context);

            Assert.That(result, Is.InstanceOf<NoSpecimen>());
        }
    }
}