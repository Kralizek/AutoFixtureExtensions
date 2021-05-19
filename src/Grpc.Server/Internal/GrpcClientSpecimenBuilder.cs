using System;
using AutoFixture;
using AutoFixture.Kernel;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Kralizek.AutoFixture.Extensions.Internal
{
    public class GrpcClientSpecimenBuilder<TClient, TEntryPoint> : ISpecimenBuilder
        where TEntryPoint : class
        where TClient : ClientBase<TClient>
    {
        public IRequestSpecification RequestSpecification { get; }

        public GrpcClientSpecimenBuilder(IRequestSpecification requestSpecification)
        {
            RequestSpecification = requestSpecification ?? throw new ArgumentNullException(nameof(requestSpecification));
        }

        public GrpcClientSpecimenBuilder() : this(new GrpcClientRequestSpecification())
        {
        }

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

            var services = new ServiceCollection();

            services.AddGrpcClient<TClient>(o => o.Address = factory.Server.BaseAddress)
                    .ConfigurePrimaryHttpMessageHandler(() => factory.Server.CreateHandler());

            var provider = services.BuildServiceProvider();

            var client = provider.GetRequiredService<TClient>();

            return client;
        }

        public class GrpcClientRequestSpecification : IRequestSpecification
        {
            public bool IsSatisfiedBy(object request) => request is Type type && type == typeof(TClient);
        }
    }
}
