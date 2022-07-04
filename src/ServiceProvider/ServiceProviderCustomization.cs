using System;
using AutoFixture;
using Kralizek.AutoFixture.Extensions.Kernel;

namespace Kralizek.AutoFixture.Extensions;

public class ServiceProviderCustomization : ICustomization
{
    private readonly IServiceProvider _serviceProvider;

    public ServiceProviderCustomization(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public void Customize(IFixture fixture)
    {
        _ = fixture ?? throw new ArgumentNullException(nameof(fixture));
        
        fixture.Inject(_serviceProvider);
        
        fixture.Customizations.Add(new ServiceProviderSpecimenBuilder(_serviceProvider));
    }
}