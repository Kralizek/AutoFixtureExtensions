using AutoFixture;
using AutoFixture.Idioms;
using Bogus;
using Kralizek.AutoFixture.Extensions.Internal;
using Moq;

namespace Tests
{
    [TestFixture]
    [TestOf(typeof(AutoFixtureBogusExtensions))]
    public class AutoFixtureBogusExtensionsTests
    {
        [TestOf(nameof(AutoFixtureBogusExtensions.CustomizeWithBogus))]
        public class CustomizeWithBogus
        {
            [Test, CustomAutoData]
            public void Throws_if_arguments_are_null(GuardClauseAssertion assertion) => assertion.Verify(typeof(AutoFixtureBogusExtensions).GetMethod(nameof(AutoFixtureBogusExtensions.CustomizeWithBogus)));

            [Test, CustomAutoData]
            public void Customizes_fixture(Func<Faker<TypeWithProperty<string>>, Faker<TypeWithProperty<string>>> customization)
            {
                var fixture = Mock.Of<IFixture>();

                AutoFixtureBogusExtensions.CustomizeWithBogus(fixture, customization);

                Mock.Get(fixture).Verify(x => x.Customize(It.IsAny<BogusCustomization<TypeWithProperty<string>>>()));
            }
        }
    }
}