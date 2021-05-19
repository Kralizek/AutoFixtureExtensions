using System;
using Grpc.Core;
using Kralizek.AutoFixture.Extensions.Internal;
using Microsoft.AspNetCore.Hosting;

// ReSharper disable CheckNamespace

namespace AutoFixture
{
    public static class GrpcServerFixtureExtensions
    {
        private static readonly Action<IWebHostBuilder> EmptyAction = _ => { };

        public static IFixture AddGrpcServerSupport<TClient, TEntryPoint>(this IFixture fixture, Action<IWebHostBuilder>? configuration = null)
            where TEntryPoint : class
            where TClient : ClientBase<TClient>
        {
            if (fixture is null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            fixture.Customize(new GrpcServerCustomization<TClient, TEntryPoint>(configuration ?? EmptyAction));

            return fixture;
        }
    }
}
