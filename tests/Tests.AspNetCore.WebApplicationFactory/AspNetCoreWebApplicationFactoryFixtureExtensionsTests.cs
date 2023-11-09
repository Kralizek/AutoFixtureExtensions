using AutoFixture;
using Kralizek.AutoFixture.Extensions.Internal;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using TestWebSite;

// ReSharper disable InvokeAsExtensionMethod

namespace Tests
{
    public class AspNetCoreWebApplicationFactoryFixtureExtensionsTests
    {
        [Test]
        public void AddWebApplicationFactorySupport_throws_if_fixture_is_null()
        {
            Assert.That(() => AspNetCoreWebApplicationFactoryFixtureExtensions.AddWebApplicationFactorySupport<Startup>(null!), Throws.ArgumentNullException);
        }
        
        [Test]
        public void AddWebApplicationFactorySupport_with_custom_factory_throws_if_fixture_is_null()
        {
            Assert.That(() => AspNetCoreWebApplicationFactoryFixtureExtensions.AddWebApplicationFactorySupport<CustomWebApplicationFactory, Startup>(null!), Throws.ArgumentNullException);
        }

        [Test]
        public void AddWebApplicationFactorySupport_with_instance_throws_if_fixture_is_null()
        {
            var instance = new WebApplicationFactory<Startup>();
            
            Assert.That(() => AspNetCoreWebApplicationFactoryFixtureExtensions.AddWebApplicationFactorySupport(null!, instance), Throws.ArgumentNullException);
        }
        
        [Test]
        public void AddWebApplicationFactorySupport_with_custom_factory_instance_throws_if_fixture_is_null()
        {
            var instance = new CustomWebApplicationFactory();
            
            Assert.That(() => AspNetCoreWebApplicationFactoryFixtureExtensions.AddWebApplicationFactorySupport<CustomWebApplicationFactory, Startup>(null!, instance), Throws.ArgumentNullException);
        }
        
        [Test, CustomAutoData]
        public void AddWebApplicationFactorySupport_registers_customization(IFixture fixture)
        {
            AspNetCoreWebApplicationFactoryFixtureExtensions.AddWebApplicationFactorySupport<Startup>(fixture);

            Assert.Multiple(() =>
            {
                Assert.That(() => fixture.Create<WebApplicationFactory<Startup>>(), Throws.Nothing);

                Assert.That(fixture.Customizations, Has.Exactly(1).InstanceOf<HttpClientSpecimenBuilder<WebApplicationFactory<Startup>, Startup>>());
            });
        }

        [Test, CustomAutoData]
        public void AddWebApplicationFactorySupport_registers_customization_with_custom_factory(IFixture fixture)
        {
            AspNetCoreWebApplicationFactoryFixtureExtensions.AddWebApplicationFactorySupport<CustomWebApplicationFactory, Startup>(fixture);

            Assert.Multiple(() =>
            {
                Assert.That(() => fixture.Create<CustomWebApplicationFactory>(), Throws.Nothing);

                Assert.That(fixture.Customizations, Has.Exactly(1).InstanceOf<HttpClientSpecimenBuilder<CustomWebApplicationFactory, Startup>>());
            });
        }
        
        [Test, CustomAutoData]
        public void AddWebApplicationFactorySupport_registers_customization_with_instance(IFixture fixture)
        {
            var instance = new CustomWebApplicationFactory();
            
            AspNetCoreWebApplicationFactoryFixtureExtensions.AddWebApplicationFactorySupport<CustomWebApplicationFactory, Startup>(fixture, instance);

            Assert.Multiple(() =>
            {
               
                Assert.That(() => fixture.Create<CustomWebApplicationFactory>(), Throws.Nothing);

                Assert.That(fixture.Customizations, Has.Exactly(1).InstanceOf<HttpClientSpecimenBuilder<CustomWebApplicationFactory, Startup>>());
            });
        }

        [Test, CustomAutoData]
        public void AddWebApplicationFactorySupport_returns_registered_instance(IFixture fixture)
        {
            var instance = new CustomWebApplicationFactory();
            
            AspNetCoreWebApplicationFactoryFixtureExtensions.AddWebApplicationFactorySupport<CustomWebApplicationFactory, Startup>(fixture, instance);

            var factory = fixture.Create<CustomWebApplicationFactory>();
            
            Assert.That(factory, Is.SameAs(instance));
        }
    }
}