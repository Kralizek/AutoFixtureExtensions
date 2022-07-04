using Kralizek.AutoFixture.Extensions;
using Moq;
// ReSharper disable InvokeAsExtensionMethod

namespace Tests;

[TestFixture]
[TestOf(typeof(FixtureExtensions))]
public class FixtureExtensionsTests
{
    [TestOf(nameof(FixtureExtensions.AddServiceProvider))]
    public class AddServiceProvider
    {
        [Test]
        [CustomAutoData]
        public void Throws_if_parameters_are_null(GuardClauseAssertion assertion) => 
            assertion
                .Verify(typeof(FixtureExtensions)
                    .GetMethod(nameof(FixtureExtensions.AddServiceProvider)));

        [Test]
        public void Adds_customization_to_fixture()
        {
            var fixture = Mock.Of<IFixture>();

            FixtureExtensions.AddServiceProvider(fixture, Mock.Of<IServiceProvider>());
            
            Mock.Get(fixture).Verify(p => p.Customize(It.IsAny<ServiceProviderCustomization>()));
        }
    }
}