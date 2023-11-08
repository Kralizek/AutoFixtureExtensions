using System;
using Kralizek.AutoFixture.Extensions.Internal;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
// ReSharper disable CheckNamespace

namespace AutoFixture
{
    public static class AspNetCoreWebApplicationFactoryFixtureExtensions
    {
        private static readonly Action<IWebHostBuilder> EmptyAction = _ => {};

        public static IFixture AddWebApplicationFactorySupport<TEntryPoint>(this IFixture fixture, Action<IWebHostBuilder>? configuration = null)
            where TEntryPoint : class
        {
            return AddWebApplicationFactorySupport<WebApplicationFactory<TEntryPoint>, TEntryPoint>(fixture, configuration ?? EmptyAction);
        }

        public static IFixture AddWebApplicationFactorySupport<TFactory, TEntryPoint>(this IFixture fixture)
            where TFactory : WebApplicationFactory<TEntryPoint>, new()
            where TEntryPoint : class
        {
            return AddWebApplicationFactorySupport<TFactory, TEntryPoint>(fixture, EmptyAction);
        }
        
        private static IFixture AddWebApplicationFactorySupport<TFactory, TEntryPoint>(this IFixture fixture, Action<IWebHostBuilder> configuration)
            where TFactory : WebApplicationFactory<TEntryPoint>, new()
            where TEntryPoint : class
        {
            if (fixture is null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            fixture.Customize(new WebApplicationFactoryCustomization<TFactory, TEntryPoint>(configuration));

            return fixture;
        }
    }
}
