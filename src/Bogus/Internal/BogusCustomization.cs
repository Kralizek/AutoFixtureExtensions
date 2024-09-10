using AutoFixture;
using AutoFixture.Kernel;
using Bogus;

namespace Kralizek.AutoFixture.Extensions.Internal
{
    public class BogusCustomization<T>(Func<Faker<T>, Faker<T>> customization) : ICustomization
        where T : class
    {
        public void Customize(IFixture fixture)
        {
            var specification = new ExactTypeSpecification(typeof(T));
            var specimenBuilder = new BogusSpecimenBuilder<T>(specification, customization);

            fixture.Customizations.Add(specimenBuilder);
        }
    }
}