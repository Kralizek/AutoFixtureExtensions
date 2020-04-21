using System;
using AutoFixture.Kernel;
using RichardSzalay.MockHttp;

namespace AutoFixture.Extensions
{
    public class HttpClientSpecimenBuilder : ISpecimenBuilder
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

            var handler = context.Create<MockHttpMessageHandler>();

            return handler.ToHttpClient();
        }
    }
}
