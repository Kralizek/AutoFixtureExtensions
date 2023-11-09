using AutoFixture;
using Kralizek.AutoFixture.Extensions.Internal;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
// ReSharper disable InvokeAsExtensionMethod

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

                Assert.That(fixture.Customizations, Has.Exactly(1).InstanceOf<HttpClientSpecimenBuilder<WebApplicationFactory<TestWebSite.Startup>, TestWebSite.Startup>>());
            });
        }

        [Test, CustomAutoData]
        public void AddWebApplicationFactorySupport_registers_customization_with_custom_factory(IFixture fixture)
        {
            AspNetCoreWebApplicationFactoryFixtureExtensions.AddWebApplicationFactorySupport<CustomWebApplicationFactory, TestWebSite.Startup>(fixture);

            Assert.Multiple(() =>
            {
                Assert.That(() => fixture.Create<CustomWebApplicationFactory>(), Throws.Nothing);

                Assert.That(fixture.Customizations, Has.Exactly(1).InstanceOf<HttpClientSpecimenBuilder<CustomWebApplicationFactory, TestWebSite.Startup>>());
            });
        }
        
        [Test, CustomAutoData]
        public void AddWebApplicationFactorySupport_registers_customization_with_instance(IFixture fixture)
        {
            var instance = new CustomWebApplicationFactory();
            
            AspNetCoreWebApplicationFactoryFixtureExtensions.AddWebApplicationFactorySupport<CustomWebApplicationFactory, TestWebSite.Startup>(fixture, instance);

            Assert.Multiple(() =>
            {
               
                Assert.That(() => fixture.Create<CustomWebApplicationFactory>(), Throws.Nothing);

                Assert.That(fixture.Customizations, Has.Exactly(1).InstanceOf<HttpClientSpecimenBuilder<CustomWebApplicationFactory, TestWebSite.Startup>>());
            });
        }

        [Test, CustomAutoData]
        public void AddWebApplicationFactorySupport_returns_registered_instance(IFixture fixture)
        {
            var instance = new CustomWebApplicationFactory();
            
            AspNetCoreWebApplicationFactoryFixtureExtensions.AddWebApplicationFactorySupport<CustomWebApplicationFactory, TestWebSite.Startup>(fixture, instance);

            var factory = fixture.Create<CustomWebApplicationFactory>();
            
            Assert.That(factory, Is.SameAs(instance));
        }
    }
}