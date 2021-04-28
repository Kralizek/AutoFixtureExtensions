// ReSharper disable CheckNamespace

using System;
using Kralizek.AutoFixture.Extensions.Internal;

namespace AutoFixture
{
    public static class GrpcAutoFixtureExtensions
    {
        public static IFixture AddGrpc(this IFixture fixture)
        {
            if (fixture is null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            fixture.Customize(new GrpcCustomization());

            return fixture;
        }
    }
}
