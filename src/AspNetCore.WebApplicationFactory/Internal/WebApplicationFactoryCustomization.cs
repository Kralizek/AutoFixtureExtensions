using System;
using AutoFixture;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Kralizek.AutoFixture.Extensions.Internal
{
    public class WebApplicationFactoryCustomization<TEntryPoint> : ICustomization
        where TEntryPoint : class
    {
        private readonly Action<IWebHostBuilder> _configuration;

        public WebApplicationFactoryCustomization(Action<IWebHostBuilder> configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void Customize(IFixture fixture)
        {
            fixture.Inject(new WebApplicationFactory<TEntryPoint>().WithWebHostBuilder(_configuration));

            fixture.Customizations.Add(new HttpClientSpecimenBuilder<TEntryPoint>());
        }
    }
}