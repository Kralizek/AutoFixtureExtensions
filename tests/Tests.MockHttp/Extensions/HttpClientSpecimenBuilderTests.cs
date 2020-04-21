using System.Net.Http;
using AutoFixture.Extensions;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using NUnit.Framework;

namespace Tests.Extensions
{
    [TestFixture]
    public class HttpClientSpecimenBuilderTests
    {
        [Test, CustomAutoData]
        public void Constructor_is_guarded(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(HttpClientSpecimenBuilder).GetConstructors());
        }

        [Test, CustomAutoData]
        public void RequestSpecification_exposes_passed_specification(IRequestSpecification requestSpecification)
        {
            var sut = new HttpClientSpecimenBuilder(requestSpecification);

            Assert.That(sut.RequestSpecification, Is.SameAs(requestSpecification));
        }

        [Test, CustomAutoData]
        public void Default_RequestSpecification_is_HttpClientRequestSpecification()
        {
            var sut = new HttpClientSpecimenBuilder();

            Assert.That(sut.RequestSpecification, Is.InstanceOf<HttpClientRequestSpecification>());
        }

        [Test, CustomAutoData]
        public void Create_is_guarded(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(HttpClientSpecimenBuilder).GetMethod(nameof(HttpClientSpecimenBuilder.Create)));
        }

        [Test, CustomAutoData]
        public void Create_returns_HttpClient_if_requested(HttpClientSpecimenBuilder sut, SpecimenContext context)
        {
            var result = sut.Create(typeof(HttpClient), context);

            Assert.That(result, Is.InstanceOf<HttpClient>());
        }

        [Test, CustomAutoData]
        public void Create_returns_NoSpecimen_if_request_is_invalid(HttpClientSpecimenBuilder sut, SpecimenContext context)
        {
            var result = sut.Create(typeof(object), context);

            Assert.That(result, Is.InstanceOf<NoSpecimen>());
        }
    }
}