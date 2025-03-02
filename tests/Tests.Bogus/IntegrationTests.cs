using AutoFixture;
using Bogus;

namespace Tests
{
    [TestFixture]
    public class IntegrationTests
    {
        [Test, CustomAutoData]
        public void Custom_value_is_used(IFixture fixture, string value)
        {
            fixture.CustomizeWithBogus<TypeWithProperty<string>>(c => c
                .RuleFor(p => p.Value, value));

            var obj = fixture.Create<TypeWithProperty<string>>();

            Assert.That(obj.Value, Is.EqualTo(value));
        }
        
        [Test, CustomAutoData]
        public void Fixture_is_used(IFixture fixture, string value)
        {
            fixture.Inject(value);
            
            fixture.CustomizeWithBogus<TypeWithProperty<string>>(c => c
                .RuleFor(p => p.Value, f => f.UseAutoFixture<string>(fixture)));

            var obj = fixture.Create<TypeWithProperty<string>>();
            
            Assert.That(obj.Value, Is.EqualTo(value));
        }
    }
}