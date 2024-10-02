using Kralizek.AutoFixture.Extensions.Internal;

namespace AutoFixture;

public static class AutoFixtureRazorPageExtensions
{
    public static IFixture AddRazorPages(this IFixture fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);
            
        fixture.Customize(new RazorPageCustomization());

        return fixture;
    }
}