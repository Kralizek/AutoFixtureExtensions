using System;
using Kralizek.AutoFixture.Extensions;

// ReSharper disable CheckNamespace

namespace AutoFixture;

public static class FixtureExtensions
{
    public static IFixture AddServiceProvider(this IFixture fixture, IServiceProvider serviceProvider)
    {
        _ = fixture ?? throw new ArgumentNullException(nameof(fixture));

        _ = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        
        fixture.Customize(new ServiceProviderCustomization(serviceProvider));

        return fixture;
    }
}
