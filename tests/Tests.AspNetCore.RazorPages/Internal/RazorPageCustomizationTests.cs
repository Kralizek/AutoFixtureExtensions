using AutoFixture;
using AutoFixture.Idioms;
using Kralizek.AutoFixture.Extensions.Internal;

namespace Tests.Internal;

[TestFixture]
[TestOf(typeof(RazorPageCustomization))]
public class RazorPageCustomizationTests
{
    [Test, CustomAutoData]
    public void Constructor_is_guarded(GuardClauseAssertion assertion) => assertion.Verify(typeof(RazorPageCustomization).GetConstructors());

    [Test, CustomAutoData]
    public void Customize_is_guarded(GuardClauseAssertion assertion) => assertion.Verify(typeof(RazorPageCustomization).GetMethods());
}