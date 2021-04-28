using System;
using AutoFixture;
using AutoFixture.Kernel;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Kralizek.AutoFixture.Extensions.Internal
{
    public class HttpClientSpecimenBuilder<TEntryPoint> : ISpecimenBuilder
        where TEntryPoint : class
    {
        public HttpClientSpecimenBuilder(IRequestSpecification requestSpecification)
        {
            RequestSpecification = requestSpecification ?? throw new ArgumentNullException(nameof(requestSpecification));
        }

        public HttpClientSpecimenBuilder() : this(new HttpClientRequestSpecification()) { }

        public IRequestSpecification RequestSpecification { get; }

        public object Create(object request, ISpecimenContext context)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (!RequestSpecification.IsSatisfiedBy(request))
            {
                return new NoSpecimen();
            }

            var factory = context.Create<WebApplicationFactory<TEntryPoint>>();

            return factory.CreateClient();
        }
    }
}
