using Kralizek.AutoFixture.Extensions;
using Kralizek.AutoFixture.Extensions.Kernel;

namespace Tests;

[TestFixture]
[TestOf(typeof(ServiceProviderCustomization))]
public class ServiceProviderCustomizationTests
{
    [Test, CustomAutoData]
    public void Throws_if_parameters_are_null(GuardClauseAssertion assertion) =>
        assertion
            .Verify(typeof(ServiceProviderCustomization).GetConstructors());
    
    [TestOf(nameof(ServiceProviderCustomization.Customize))]
    public class Customize
    {
        [Test, CustomAutoData]
        public void Throws_if_parameters_are_null(GuardClauseAssertion assertion) =>
            assertion
                .Verify(typeof(ServiceProviderCustomization)
                    .GetMethod(nameof(ServiceProviderCustomization.Customize)));

        [Test, CustomAutoData]
        public void Injects_given_instance_of_service_provider([Frozen] IServiceProvider serviceProvider, ServiceProviderCustomization sut)
        {
            var fixture = new Fixture();
            
            sut.Customize(fixture);

            var instance = fixture.Create<IServiceProvider>();
            
            Assert.That(instance, Is.SameAs(serviceProvider));
        }

        [Test, CustomAutoData]
        public void Adds_specimen_builder(ServiceProviderCustomization sut)
        {
            var fixture = new Fixture();
            
            sut.Customize(fixture);
            
            Assert.That(fixture.Customizations, Has.Exactly(1).InstanceOf<ServiceProviderSpecimenBuilder>());
        }
    }
}