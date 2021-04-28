using System;
using AutoFixture;
using AutoFixture.Kernel;
using Grpc.Core;

namespace Kralizek.AutoFixture.Extensions.Internal
{
    public class GrpcCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            if (fixture is null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            fixture.Customizations.Add(new TypeRelay(typeof(IAsyncStreamReader<>), typeof(MockAsyncStreamReader<>)));

            fixture.Customizations.Add(new GrpcAsyncCallSpecimenBuilder());
        }
    }
}
