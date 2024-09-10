using AutoFixture.Kernel;
using Bogus;

namespace Kralizek.AutoFixture.Extensions.Internal
{
    public class BogusSpecimenBuilder<T>(IRequestSpecification requestSpecification, Func<Faker<T>, Faker<T>> customization) : ISpecimenBuilder
        where T : class
    {
        public object Create(object request, ISpecimenContext context)
        {
            ArgumentNullException.ThrowIfNull(request);
            ArgumentNullException.ThrowIfNull(context);

            if (!requestSpecification.IsSatisfiedBy(request))
            {
                return new NoSpecimen();
            }

            var faker = new Faker<T>();
            faker = customization(faker);

            return faker.Generate();
        }
    }
}