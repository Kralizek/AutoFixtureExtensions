using AutoFixture;
using AutoFixture.Idioms;
using Kralizek.AutoFixture.Extensions.Internal;
using Moq;

namespace Tests;

[TestFixture]
[TestOf(typeof(AutoFixtureRazorPageExtensions))]
public class AutoFixtureRazorPageExtensionsTests
{
    [TestOf(nameof(AutoFixtureRazorPageExtensions.AddRazorPages))]
    public class AddRazorPages
    {
        [Test, CustomAutoData]
        public void Throws_if_arguments_are_null(GuardClauseAssertion assertion) => assertion.Verify(typeof(AutoFixtureRazorPageExtensions).GetMethod(nameof(AutoFixtureRazorPageExtensions.AddRazorPages)));
        
        [Test, CustomAutoData]
        public void Customizes_fixture()
        {
            var fixture = Mock.Of<IFixture>();

            fixture.AddRazorPages();

            Mock.Get(fixture).Verify(x => x.Customize(It.IsAny<RazorPageCustomization>()));
        }
    }
}