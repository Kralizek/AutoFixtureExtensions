using Bogus;
using Kralizek.AutoFixture.Extensions.Internal;

namespace AutoFixture
{
    public static class AutoFixtureBogusExtensions
    {
        public static IFixture CustomizeWithBogus<T>(this IFixture fixture, Func<Faker<T>, Faker<T>> customization)
            where T : class
        {
            ArgumentNullException.ThrowIfNull(fixture);
            ArgumentNullException.ThrowIfNull(customization);

            fixture.Customize(new BogusCustomization<T>(customization));
            
            return fixture;
        }
    }
}