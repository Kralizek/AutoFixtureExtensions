using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Kernel;
using Grpc.Core;
using Grpc.Core.Testing;
using Kralizek.AutoFixture.Extensions.Internal;

namespace Kralizek.AutoFixture.Extensions
{
    public class GrpcCustomization : ICustomization
    {
        private readonly GrpcCustomizationOptions _options;

        public GrpcCustomization(GrpcCustomizationOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public void Customize(IFixture fixture)
        {
            if (fixture is null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            fixture.Customizations.Add(new TypeRelay(typeof(IAsyncStreamReader<>), typeof(MockAsyncStreamReader<>)));

            fixture.Customizations.Add(new GrpcAsyncCallSpecimenBuilder(_options.CallParameters));

            fixture.Customize<ServerCallContext>(o => o.FromFactory(_options.ServerCallContextFactory));
        }
    }

    public class GrpcCustomizationOptions
    {
        public static readonly CallParameters DefaultCallParameters = new CallParameters(Task.FromResult(new Metadata()), () => Status.DefaultSuccess, () => new Metadata(), () => {});

        public static readonly Func<IFixture, ServerCallContext> DefaultServerCallContextFactory = (IFixture f) => TestServerCallContext.Create(
            f.Create<string>(),
            null,
            DateTime.UtcNow.AddHours(1),
            new Metadata(),
            CancellationToken.None,
            "127.0.0.1",
            null,
            null,
            _ => Task.CompletedTask,
            () => new WriteOptions(),
            _ => { });

        public CallParameters CallParameters { get; set; } = DefaultCallParameters;

        public Func<IFixture, ServerCallContext> ServerCallContextFactory { get; set; } = DefaultServerCallContextFactory;
    }
}
