using AutoFixture;

namespace Tests
{
    [TestFixture]
    public class IntegrationTests
    {
        [Test, CustomAutoData]
        public void Custom_value_is_used(IFixture fixture, string value)
        {
            fixture.CustomizeWithBogus<TypeWithProperty<string>>(c => c.RuleFor(p => p.Value, () => value));

            var obj = fixture.Create<TypeWithProperty<string>>();

            Assert.That(obj.Value, Is.EqualTo(value));
        }
    }
}