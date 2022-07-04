using AutoFixture.Kernel;
using Kralizek.AutoFixture.Extensions.Kernel;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Kernel;

[TestFixture]
[TestOf(typeof(ServiceProviderSpecimenBuilder))]
public class ServiceProviderSpecimenBuilderTests
{
    [Test, CustomAutoData]
    public void Throws_if_parameters_are_null(GuardClauseAssertion assertion) =>
        assertion
            .Verify(typeof(ServiceProviderSpecimenBuilder).GetConstructors());

    [TestOf(nameof(ServiceProviderSpecimenBuilder.Create))]
    public class Create
    {
        [Test, CustomAutoData]
        public void Throws_if_parameters_are_null(GuardClauseAssertion assertion) =>
            assertion
                .Verify(typeof(ServiceProviderSpecimenBuilder)
                    .GetMethod(nameof(ServiceProviderSpecimenBuilder.Create)));
        
        [Test, CustomAutoData]
        public void Returns_NoSpecimen_if_request_is_not_type(ServiceProviderSpecimenBuilder sut, SpecimenContext context)
        {
            var result = sut.Create(new object(), context);
            
            Assert.That(result, Is.InstanceOf<NoSpecimen>());
        }

        [Test, CustomAutoData]
        public void Returns_NoSpecimen_if_type_is_not_registered(ServiceProviderSpecimenBuilder sut, SpecimenContext context, Type type)
        {
            var result = sut.Create(type, context);
            
            Assert.That(result, Is.InstanceOf<NoSpecimen>());
        }

        [Test, CustomAutoData]
        public void Returns_registered_service_if_available(IServiceCollection services, SpecimenContext context)
        {
            services.AddSingleton<IEcho, EchoService>();

            var serviceProvider = services.BuildServiceProvider();

            var sut = new ServiceProviderSpecimenBuilder(serviceProvider);

            var result = sut.Create(typeof(IEcho), context);
            
            Assert.That(result, Is.InstanceOf<EchoService>());
        }
    }
}