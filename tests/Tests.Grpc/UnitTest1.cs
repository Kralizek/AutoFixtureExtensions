using AutoFixture;
using Grpc.Core;
using Kralizek.AutoFixture.Extensions.Internal;
using NUnit.Framework;

namespace Tests
{
    public class GrpcAutoFixtureExtensionsTests
    {
        [Test, CustomAutoData]
        public void AddGrpc_registers_customization(IFixture fixture)
        {
            fixture.AddGrpc();

            Assert.Multiple(() =>
            {
                Assert.That(fixture.Create<IAsyncStreamReader<string>>(), Is.InstanceOf<MockAsyncStreamReader<string>>());

                Assert.That(() => fixture.Create<AsyncUnaryCall<string>>(), Throws.Nothing);

                Assert.That(() => fixture.Create<AsyncClientStreamingCall<string, string>>(), Throws.Nothing);

                Assert.That(() => fixture.Create<AsyncServerStreamingCall<string>>(), Throws.Nothing);

                Assert.That(() => fixture.Create<AsyncDuplexStreamingCall<string, string>>(), Throws.Nothing);
            });
        }
    }
}