using AutoFixture;
using Kralizek.AutoFixture.Extensions.Internal;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace Tests
{
    public class AspNetCoreWebApplicationFactoryFixtureExtensionsTests
    {
        [Test, CustomAutoData]
        public void AddWebApplicationFactorySupport_registers_customization(IFixture fixture)
        {
            AspNetCoreWebApplicationFactoryFixtureExtensions.AddWebApplicationFactorySupport<TestWebSite.Startup>(fixture);

            Assert.Multiple(() =>
            {
                Assert.That(() => fixture.Create<WebApplicationFactory<TestWebSite.Startup>>(), Throws.Nothing);

                Assert.That(fixture.Customizations, Has.Exactly(1).InstanceOf<HttpClientSpecimenBuilder<TestWebSite.Startup>>());
            });
        }
    }
}