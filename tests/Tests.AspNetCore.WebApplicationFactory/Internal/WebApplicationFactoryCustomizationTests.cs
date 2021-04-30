using System.Linq;
using AutoFixture;
using AutoFixture.Idioms;
using Kralizek.AutoFixture.Extensions.Internal;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace Tests.Internal
{
    [TestFixture]
    public class WebApplicationFactoryCustomizationTests
    {
        [Test, CustomAutoData]
        public void Constructor_does_not_accept_nulls(GuardClauseAssertion assertion) => assertion.Verify(typeof(WebApplicationFactoryCustomization<TestWebSite.Startup>));

        [Test, CustomAutoData]
        public void Customize_injects_instance_of_WebApplicationFactory(WebApplicationFactoryCustomization<TestWebSite.Startup> sut, IFixture fixture)
        {
            sut.Customize(fixture);

            Assert.That(() => fixture.Create<WebApplicationFactory<TestWebSite.Startup>>(), Throws.Nothing);
        }

        [Test, CustomAutoData]
        public void Multiple_requests_for_WebApplicationFactory_return_same_instance(WebApplicationFactoryCustomization<TestWebSite.Startup> sut, IFixture fixture)
        {
            sut.Customize(fixture);

            var first = fixture.Create<WebApplicationFactory<TestWebSite.Startup>>();

            var second = fixture.Create<WebApplicationFactory<TestWebSite.Startup>>();

            Assert.That(first, Is.SameAs(second));
        }

        [Test, CustomAutoData]
        public void Customize_registers_HttpClientSpecimenBuilder(WebApplicationFactoryCustomization<TestWebSite.Startup> sut, IFixture fixture)
        {
            sut.Customize(fixture);

            Assert.That(fixture.Customizations, Has.Exactly(1).InstanceOf<HttpClientSpecimenBuilder<TestWebSite.Startup>>());
        }
    }
}
