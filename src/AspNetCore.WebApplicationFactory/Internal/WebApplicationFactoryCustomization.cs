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
        private readonly TFactory? _instance;
        private readonly Action<IWebHostBuilder> _configuration;

        public WebApplicationFactoryCustomization(TFactory? instance, Action<IWebHostBuilder> configuration)
        {
            _instance = instance;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void Customize(IFixture fixture)
        {
            if (_instance is not null)
            {
                fixture.Inject<TFactory>(_instance);
            }
            else if (typeof(TFactory) == typeof(WebApplicationFactory<TEntryPoint>))
            {
                fixture.Inject(new TFactory().WithWebHostBuilder(_configuration));
            }
            else
            {
                fixture.Inject(new TFactory());
            }
            
            fixture.Customizations.Add(new HttpClientSpecimenBuilder<TFactory, TEntryPoint>());
        }
    }
}