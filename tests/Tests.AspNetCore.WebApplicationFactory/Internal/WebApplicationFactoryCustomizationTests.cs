using System;
using AutoFixture;
using Kralizek.AutoFixture.Extensions.Internal;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using TestWebSite;

namespace Tests.Internal
{
    [TestFixture]
    public class WebApplicationFactoryCustomizationTests
    {
        private static readonly Action<IWebHostBuilder> Empty = _ => { };

        [Test]
        public void Configuration_delegate_is_required()
        {
            Assert.That(() => new WebApplicationFactoryCustomization<WebApplicationFactory<Startup>, Startup>(null, configuration: null!), Throws.ArgumentNullException);
        }

        [Test, CustomAutoData]
        public void Customize_injects_instance_of_WebApplicationFactory(IFixture fixture)
        {
            var sut = new WebApplicationFactoryCustomization<WebApplicationFactory<Startup>, Startup>(null, Empty);
            
            sut.Customize(fixture);

            Assert.That(() => fixture.Create<WebApplicationFactory<Startup>>(), Throws.Nothing);
        }
        
        [Test, CustomAutoData]
        public void Customize_injects_instance_of_custom_WebApplicationFactory(IFixture fixture)
        {
            var sut = new WebApplicationFactoryCustomization<CustomWebApplicationFactory, Startup>(null, Empty);
            
            sut.Customize(fixture);

            Assert.That(() => fixture.Create<CustomWebApplicationFactory>(), Throws.Nothing);
        }
        
        [Test, CustomAutoData]
        public void Customize_injects_specific_instance_of_WebApplicationFactory(IFixture fixture)
        {
            var instance = new WebApplicationFactory<Startup>();
            
            var sut = new WebApplicationFactoryCustomization<WebApplicationFactory<Startup>, Startup>(instance, Empty);
            
            sut.Customize(fixture);

            Assert.That(() => fixture.Create<WebApplicationFactory<Startup>>(), Is.SameAs(instance));
        }
        
        [Test, CustomAutoData]
        public void Customize_injects_specific_instance_of_custom_WebApplicationFactory(IFixture fixture)
        {
            var instance = new CustomWebApplicationFactory();
            
            var sut = new WebApplicationFactoryCustomization<CustomWebApplicationFactory, Startup>(instance, Empty);
            
            sut.Customize(fixture);

            Assert.That(() => fixture.Create<CustomWebApplicationFactory>(), Is.SameAs(instance));
        }
        
        [Test, CustomAutoData]
        public void Customize_injects_specific_instance_of_custom_WebApplicationFactory_as_standard(IFixture fixture)
        {
            var instance = new CustomWebApplicationFactory();
            
            var sut = new WebApplicationFactoryCustomization<WebApplicationFactory<Startup>, Startup>(instance, Empty);
            
            sut.Customize(fixture);

            Assert.That(() => fixture.Create<WebApplicationFactory<Startup>>(), Is.SameAs(instance));
        }

        [Test, CustomAutoData]
        public void Multiple_requests_for_WebApplicationFactory_return_same_instance(IFixture fixture)
        {
            var sut = new WebApplicationFactoryCustomization<WebApplicationFactory<Startup>, Startup>(null, Empty);
            
            sut.Customize(fixture);

            var first = fixture.Create<WebApplicationFactory<Startup>>();

            var second = fixture.Create<WebApplicationFactory<Startup>>();

            Assert.That(first, Is.SameAs(second));
        }
        
        [Test, CustomAutoData]
        public void Multiple_requests_for_custom_WebApplicationFactory_return_same_instance(IFixture fixture)
        {
            var sut = new WebApplicationFactoryCustomization<CustomWebApplicationFactory, Startup>(null, Empty);
            
            sut.Customize(fixture);

            var first = fixture.Create<CustomWebApplicationFactory>();

            var second = fixture.Create<CustomWebApplicationFactory>();

            Assert.That(first, Is.SameAs(second));
        }

        [Test, CustomAutoData]
        public void Multiple_requests_for_specific_instance_of_WebApplicationFactory_return_same_instance(IFixture fixture)
        {
            var instance = new WebApplicationFactory<Startup>();
            
            var sut = new WebApplicationFactoryCustomization<WebApplicationFactory<Startup>, Startup>(instance, Empty);
            
            sut.Customize(fixture);

            var first = fixture.Create<WebApplicationFactory<Startup>>();

            var second = fixture.Create<WebApplicationFactory<Startup>>();

            Assert.That(first, Is.SameAs(second));
        }
        
        [Test, CustomAutoData]
        public void Multiple_requests_for_specific_instance_of_custom_WebApplicationFactory_return_same_instance(IFixture fixture)
        {
            var instance = new CustomWebApplicationFactory();
            
            var sut = new WebApplicationFactoryCustomization<CustomWebApplicationFactory, Startup>(instance, Empty);
            
            sut.Customize(fixture);

            var first = fixture.Create<CustomWebApplicationFactory>();

            var second = fixture.Create<CustomWebApplicationFactory>();

            Assert.That(first, Is.SameAs(second));
        }
        
        [Test, CustomAutoData]
        public void Multiple_requests_for_specific_instance_of_custom_WebApplicationFactory_as_standard_return_same_instance(IFixture fixture)
        {
            var instance = new CustomWebApplicationFactory();
            
            var sut = new WebApplicationFactoryCustomization<WebApplicationFactory<Startup>, Startup>(instance, Empty);
            
            sut.Customize(fixture);

            var first = fixture.Create<WebApplicationFactory<Startup>>();

            var second = fixture.Create<WebApplicationFactory<Startup>>();

            Assert.That(first, Is.SameAs(second));
        }

        [Test, CustomAutoData]
        public void Customize_registers_HttpClientSpecimenBuilder(IFixture fixture)
        {
            var sut = new WebApplicationFactoryCustomization<WebApplicationFactory<Startup>, Startup>(null, Empty);
            
            sut.Customize(fixture);

            Assert.That(fixture.Customizations, Has.Exactly(1).InstanceOf<HttpClientSpecimenBuilder<WebApplicationFactory<Startup>, Startup>>());
        }
        
        [Test, CustomAutoData]
        public void Customize_registers_HttpClientSpecimenBuilder_for_custom_factory(IFixture fixture)
        {
            var sut = new WebApplicationFactoryCustomization<CustomWebApplicationFactory, Startup>(null, Empty);
            
            sut.Customize(fixture);

            Assert.That(fixture.Customizations, Has.Exactly(1).InstanceOf<HttpClientSpecimenBuilder<CustomWebApplicationFactory, Startup>>());
        }
        
        [Test, CustomAutoData]
        public void Customize_registers_HttpClientSpecimenBuilder_with_instance(IFixture fixture)
        {
            var instance = new WebApplicationFactory<Startup>();
            
            var sut = new WebApplicationFactoryCustomization<WebApplicationFactory<Startup>, Startup>(instance, Empty);
            
            sut.Customize(fixture);

            Assert.That(fixture.Customizations, Has.Exactly(1).InstanceOf<HttpClientSpecimenBuilder<WebApplicationFactory<Startup>, Startup>>());
        }
        
        [Test, CustomAutoData]
        public void Customize_registers_HttpClientSpecimenBuilder_for_custom_factory_with_instance(IFixture fixture)
        {
            var instance = new CustomWebApplicationFactory();
            
            var sut = new WebApplicationFactoryCustomization<CustomWebApplicationFactory, Startup>(instance, Empty);
            
            sut.Customize(fixture);

            Assert.That(fixture.Customizations, Has.Exactly(1).InstanceOf<HttpClientSpecimenBuilder<CustomWebApplicationFactory, Startup>>());
        }
        
        [Test, CustomAutoData]
        public void Customize_registers_HttpClientSpecimenBuilder_for_custom_factory_with_instance_as_standard(IFixture fixture)
        {
            var instance = new CustomWebApplicationFactory();
            
            var sut = new WebApplicationFactoryCustomization<WebApplicationFactory<Startup>, Startup>(instance, Empty);
            
            sut.Customize(fixture);

            Assert.That(fixture.Customizations, Has.Exactly(1).InstanceOf<HttpClientSpecimenBuilder<WebApplicationFactory<Startup>, Startup>>());
        }
    }
}
