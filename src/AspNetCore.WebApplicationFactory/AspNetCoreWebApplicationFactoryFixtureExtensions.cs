using System;
using Kralizek.AutoFixture.Extensions.Internal;
using Microsoft.AspNetCore.Hosting;
// ReSharper disable CheckNamespace

namespace AutoFixture
{
    public static class AspNetCoreWebApplicationFactoryFixtureExtensions
    {
        private static readonly Action<IWebHostBuilder> EmptyAction = _ => {};

        public static IFixture AddWebApplicationFactorySupport<TEntryPoint>(this IFixture fixture, Action<IWebHostBuilder>? configuration = null)
            where TEntryPoint : class
        {
            if (fixture is null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            fixture.Customize(new WebApplicationFactoryCustomization<TEntryPoint>(configuration ?? EmptyAction));

            return fixture;
        }
    }
}
