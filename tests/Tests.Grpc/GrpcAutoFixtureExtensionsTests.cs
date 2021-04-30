using AutoFixture;
using Grpc.Core;
using Grpc.Core.Testing;
using Kralizek.AutoFixture.Extensions.Internal;
using NUnit.Framework;

namespace Tests
{
    public class GrpcAutoFixtureExtensionsTests
    {
        [Test, CustomAutoData]
        public void AddGrpcSupport_registers_customization(IFixture fixture)
        {
            fixture.AddGrpcSupport();

            Assert.Multiple(() =>
            {
                Assert.That(fixture.Create<IAsyncStreamReader<string>>(), Is.InstanceOf<MockAsyncStreamReader<string>>());

                Assert.That(() => fixture.Create<AsyncUnaryCall<string>>(), Throws.Nothing);

                Assert.That(() => fixture.Create<AsyncClientStreamingCall<string, string>>(), Throws.Nothing);

                Assert.That(() => fixture.Create<AsyncServerStreamingCall<string>>(), Throws.Nothing);

                Assert.That(() => fixture.Create<AsyncDuplexStreamingCall<string, string>>(), Throws.Nothing);

                Assert.That(fixture.Create<ServerCallContext>(), Is.Not.Null);
            });
        }
    }
}