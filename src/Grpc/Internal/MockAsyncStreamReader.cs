using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;

namespace Kralizek.AutoFixture.Extensions.Internal
{
    public class MockAsyncStreamReader<T> : IAsyncStreamReader<T>, IDisposable
    {
        private readonly IEnumerator<T> _enumerator;

        public MockAsyncStreamReader(IEnumerable<T> data)
        {
            _enumerator = data?.GetEnumerator() ?? throw new ArgumentNullException(nameof(data));
        }

        public Task<bool> MoveNext(CancellationToken cancellationToken) => Task.FromResult(_enumerator.MoveNext());

        public T Current => _enumerator.Current;

        public void Dispose()
        {
            _enumerator?.Dispose();
        }
    }
}