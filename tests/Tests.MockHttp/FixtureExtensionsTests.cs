using NUnit.Framework;
using AutoFixture;
using System;
using AutoFixture.Extensions;

namespace Tests
{
    [TestFixture]
    public class FixtureExtensionsTests
    {
        [Test]
        public void AddMockHttp_throws_if_fixture_is_null()
        {
            IFixture fixture = null;
            
            Assert.Throws<ArgumentNullException>(() => fixture.AddMockHttp());    
        }

        [Test, CustomAutoData]
        public void AddMockHttp_returns_same_fixture(IFixture fixture)
        {
            var result = fixture.AddMockHttp();

            Assert.That(result, Is.SameAs(fixture));
        }

        [Test, CustomAutoData]
        public void AddMockHttp_adds_HttpClientSpecimenBuilder_to_fixture_Customizations(IFixture fixture)
        {
            fixture.AddMockHttp();

            Assert.That(fixture.Customizations, Has.One.InstanceOf<HttpClientSpecimenBuilder>());
        }
    }
}