using System;
using AutoFixture.Extensions;

namespace AutoFixture
{
    public static class FixtureExtensions
    {
        public static IFixture AddMockHttp(this IFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            fixture.Customizations.Add(new HttpClientSpecimenBuilder());

            return fixture;
        }
    }
}