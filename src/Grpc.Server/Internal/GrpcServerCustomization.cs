using System;
using AutoFixture;
using Grpc.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Kralizek.AutoFixture.Extensions.Internal
{
    public class GrpcServerCustomization<TClient, TEntryPoint> : ICustomization
        where TEntryPoint : class
        where TClient : ClientBase<TClient>

    {
        private readonly Action<IWebHostBuilder> _configuration;

        public GrpcServerCustomization(Action<IWebHostBuilder> configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void Customize(IFixture fixture)
        {
            fixture.Inject(new WebApplicationFactory<TEntryPoint>().WithWebHostBuilder(_configuration));

            fixture.Customizations.Add(new GrpcClientSpecimenBuilder<TClient, TEntryPoint>());
        }
    }
}
