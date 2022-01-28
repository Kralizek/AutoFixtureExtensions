using AutoFixture;
using Kralizek.AutoFixture.Extensions.Kernel;
using NUnit.Framework;

namespace Tests
{

    public class AsyncEnumerableCustomizationTests
    {
        [Test, CustomAutoData]
        public void Customize_adds_specimen_builder(AsyncEnumerableCustomization sut, IFixture fixture)
        {
            sut.Customize(fixture);

            Assert.That(fixture.Customizations, Has.Exactly(1).InstanceOf<AsyncEnumerableSpecimenBuilder>());
        }

    }
}