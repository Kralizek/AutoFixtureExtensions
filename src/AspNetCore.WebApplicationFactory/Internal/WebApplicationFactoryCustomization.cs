using System;
using AutoFixture;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Kralizek.AutoFixture.Extensions.Internal
{
    public class WebApplicationFactoryCustomization<TFactory, TEntryPoint> : ICustomization
        where TFactory : WebApplicationFactory<TEntryPoint>, new()
        where TEntryPoint : class
    {
        private readonly Action<IWebHostBuilder> _configuration;

        public WebApplicationFactoryCustomization(Action<IWebHostBuilder> configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void Customize(IFixture fixture)
        {
            fixture.Inject(new TFactory().WithWebHostBuilder(_configuration));

            fixture.Customizations.Add(new HttpClientSpecimenBuilder<TFactory, TEntryPoint>());
        }
    }
}