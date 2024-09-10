using AutoFixture;
using AutoFixture.Idioms;
using Kralizek.AutoFixture.Extensions.Internal;

namespace Tests.Internal
{
    [TestFixture]
    [TestOf(typeof(BogusCustomization<>))]
    public class BogusCustomizationTests
    {
        [Test, CustomAutoData]
        public void Constructor_is_guarded(GuardClauseAssertion assertion) => assertion.Verify(typeof(BogusCustomization<>).GetConstructors());

        [Test, CustomAutoData]
        public void Customize_is_guarded(GuardClauseAssertion assertion) => assertion.Verify(typeof(BogusCustomization<>).GetMethods());

        [Test, CustomAutoData]
        public void Customize_adds_specimen_builder(BogusCustomization<TypeWithProperty<string>> sut, IFixture fixture)
        {
            sut.Customize(fixture);

            Assert.That(fixture.Customizations, Has.Exactly(1).InstanceOf<BogusSpecimenBuilder<TypeWithProperty<string>>>());
        }
    }
}
