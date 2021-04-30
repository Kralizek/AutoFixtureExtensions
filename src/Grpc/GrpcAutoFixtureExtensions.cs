// ReSharper disable CheckNamespace

using System;
using Kralizek.AutoFixture.Extensions;

namespace AutoFixture
{
    public static class GrpcAutoFixtureExtensions
    {
        public static IFixture AddGrpcSupport(this IFixture fixture, GrpcCustomizationOptions? options = null)
        {
            if (fixture is null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            fixture.Customize(new GrpcCustomization(options ?? new GrpcCustomizationOptions()));

            return fixture;
        }
    }
}
