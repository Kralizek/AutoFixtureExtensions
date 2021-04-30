using System;
using AutoFixture;
using AutoFixture.Kernel;
using Grpc.Core;
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
}
