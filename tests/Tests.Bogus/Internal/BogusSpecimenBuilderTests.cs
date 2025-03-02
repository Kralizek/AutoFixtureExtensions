using AutoFixture.Idioms;
using AutoFixture.Kernel;
using AutoFixture.NUnit3;
using Bogus;
using Kralizek.AutoFixture.Extensions.Internal;
using Moq;

namespace Tests.Internal
{
    [TestFixture]
    [TestOf(typeof(BogusSpecimenBuilder<>))]
    public class BogusSpecimenBuilderTests
    {
        [Test, CustomAutoData]
        public void Constructor_is_guarded(GuardClauseAssertion assertion) => assertion.Verify(typeof(BogusSpecimenBuilder<>).GetConstructors());

        [Test, CustomAutoData]
        public void Customize_is_guarded(GuardClauseAssertion assertion) => assertion.Verify(typeof(BogusSpecimenBuilder<>).GetMethods());

        [Test, CustomAutoData]
        public void Customize_returns_NoSpecimen_if_specification_is_false([Frozen] IRequestSpecification specification, BogusSpecimenBuilder<TypeWithProperty<string>> sut, object request, ISpecimenContext context)
        {
            Mock.Get(specification).Setup(x => x.IsSatisfiedBy(It.IsAny<object>())).Returns(false);
            
            var result = sut.Create(request, context);

            Assert.That(result, Is.InstanceOf<NoSpecimen>());
        }

        [Test, CustomAutoData]
        public void Uses_customization_delegate_when_generating_object([Frozen] IRequestSpecification specification, [Frozen] Func<Faker<TypeWithProperty<string>>, Faker<TypeWithProperty<string>>> customization, BogusSpecimenBuilder<TypeWithProperty<string>> sut, object request, ISpecimenContext context)
        {
            Mock.Get(specification).Setup(x => x.IsSatisfiedBy(It.IsAny<object>())).Returns(true);

            var result = sut.Create(request, context);

            Mock.Get(customization).Verify(x => x.Invoke(It.IsAny<Faker<TypeWithProperty<string>>>()));
        }
    }
}
