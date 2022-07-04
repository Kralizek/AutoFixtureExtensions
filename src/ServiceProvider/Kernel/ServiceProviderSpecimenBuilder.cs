using System;
using AutoFixture.Kernel;

namespace Kralizek.AutoFixture.Extensions.Kernel;

public class ServiceProviderSpecimenBuilder : ISpecimenBuilder
{
    private readonly IServiceProvider _serviceProvider;

    public ServiceProviderSpecimenBuilder(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }
    
    public object Create(object request, ISpecimenContext context)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        _ = context ?? throw new ArgumentNullException(nameof(context));
        
        if (request is not Type type)
        {
            return new NoSpecimen();
        }

        var service = _serviceProvider.GetService(type);

        if (service is null)
        {
            return new NoSpecimen();
        }

        return service;
    }
}