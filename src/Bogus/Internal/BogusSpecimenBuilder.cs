using AutoFixture;
using AutoFixture.Kernel;
using Bogus;

namespace Kralizek.AutoFixture.Extensions.Internal;

public class BogusSpecimenBuilder<T> : ISpecimenBuilder
    where T : class
{
    private readonly IRequestSpecification _requestSpecification;
    private readonly Func<Faker<T>, Faker<T>> _customization;

    public BogusSpecimenBuilder(IRequestSpecification requestSpecification, Func<Faker<T>, Faker<T>> customization)
    {
        _requestSpecification = requestSpecification ?? throw new ArgumentNullException(nameof(requestSpecification));
        _customization = customization ?? throw new ArgumentNullException(nameof(customization));
    }

    public object Create(object request, ISpecimenContext context)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(context);

        if (!_requestSpecification.IsSatisfiedBy(request))
        {
            return new NoSpecimen();
        }

        var faker = new Faker<T>();
        faker = _customization(faker);

        var item = faker.Generate();

        return item;
    }
}
