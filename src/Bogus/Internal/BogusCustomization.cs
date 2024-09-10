using AutoFixture;
using AutoFixture.Kernel;
using Bogus;

namespace Kralizek.AutoFixture.Extensions.Internal
{
    public class BogusCustomization<T>(Func<Faker<T>, Faker<T>> customization) : ICustomization
        where T : class
    {
        private readonly Func<Faker<T>, Faker<T>> _customization = customization ?? throw new ArgumentNullException(nameof(customization));

        public void Customize(IFixture fixture)
        {
            ArgumentNullException.ThrowIfNull(fixture);

            var specification = new ExactTypeSpecification(typeof(T));
            var specimenBuilder = new BogusSpecimenBuilder<T>(specification, _customization);

            fixture.Customizations.Add(specimenBuilder);
        }
    }
}