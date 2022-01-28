using AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kralizek.AutoFixture.Extensions.Kernel
{
    public class AsyncEnumerableSpecimenBuilder : ISpecimenBuilder
    {
        public IRequestSpecification RequestSpecification { get; }

        public AsyncEnumerableSpecimenBuilder(IRequestSpecification requestSpecification)
        {
            RequestSpecification = requestSpecification ?? throw new ArgumentNullException(nameof(requestSpecification));
        }

        public AsyncEnumerableSpecimenBuilder() : this (new AsyncEnumerableRequestSpecification()) {}

        public object Create(object request, ISpecimenContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request is not Type type || !RequestSpecification.IsSatisfiedBy(request))
            {
                return new NoSpecimen();
            }

            var typeArgument = type.GenericTypeArguments[0];

            var arrayType = typeArgument.MakeArrayType();

            var items = context.Resolve(arrayType);

            var instance = Activator.CreateInstance(typeof(SynchronousAsyncEnumerable<>).MakeGenericType(typeArgument), args: new[] { items });

            if (instance is null)
            {
                return new NoSpecimen();
            }

            return instance;
        }

        private class SynchronousAsyncEnumerable<T> : IAsyncEnumerable<T>
        {
            private readonly IEnumerable<T> _enumerable;

            public SynchronousAsyncEnumerable(IEnumerable<T> enumerable) =>
                _enumerable = enumerable;

            public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default) =>
                new SynchronousAsyncEnumerator<T>(_enumerable.GetEnumerator());
        }

        private class SynchronousAsyncEnumerator<T> : IAsyncEnumerator<T>
        {
            private readonly IEnumerator<T> _enumerator;

            public T Current => _enumerator.Current;

            public SynchronousAsyncEnumerator(IEnumerator<T> enumerator) =>
                _enumerator = enumerator;

            public ValueTask DisposeAsync() => new ValueTask(Task.CompletedTask);

            public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(Task.FromResult(_enumerator.MoveNext()));
        }

        public class AsyncEnumerableRequestSpecification : IRequestSpecification
        {
            public bool IsSatisfiedBy(object request)
            {
                return request is Type { IsGenericType: true } type && type.GetGenericTypeDefinition() == typeof(IAsyncEnumerable<>);
            }
        }
    }
}