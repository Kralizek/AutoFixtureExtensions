using System;
using System.Reflection;
using System.Threading.Tasks;
using AutoFixture.Kernel;
using Grpc.Core;
using Grpc.Core.Testing;

namespace Kralizek.AutoFixture.Extensions.Internal
{
    public class GrpcAsyncCallSpecimenBuilder : ISpecimenBuilder
    {
        private static readonly CallParameters DefaultCallParameters = new CallParameters(Task.FromResult(new Metadata()), () => Status.DefaultSuccess, () => new Metadata(), () => {});

        public object Create(object request, ISpecimenContext context) => request switch
        {
            null => throw new ArgumentNullException(nameof(request)),
            Type { IsGenericType: true } type when type.GetGenericTypeDefinition() == typeof(AsyncServerStreamingCall<>) => CreateAsyncServerStreamingCall(type, context),
            Type { IsGenericType: true } type when type.GetGenericTypeDefinition() == typeof(AsyncUnaryCall<>) => CreateAsyncUnaryCall(type, context),
            Type { IsGenericType: true } type when type.GetGenericTypeDefinition() == typeof(AsyncClientStreamingCall<,>) => CreateAsyncClientStreamingCall(type, context),
            Type { IsGenericType: true } type when type.GetGenericTypeDefinition() == typeof(AsyncDuplexStreamingCall<,>) => CreateAsyncDuplexStreamingCall(type, context),
            _ => new NoSpecimen()
        };

        private static object CreateAsyncServerStreamingCall(Type type, ISpecimenContext context)
        {
            var typeArgument = type.GenericTypeArguments[0];

            var asyncStreamReaderType = typeof(IAsyncStreamReader<>).MakeGenericType(typeArgument);

            var responseStream = context.Resolve(asyncStreamReaderType);

            var method = typeof(TestCalls).GetMethod(nameof(TestCalls.AsyncServerStreamingCall)).MakeGenericMethod(typeArgument);

            var result = method.Invoke(null, new[] { responseStream, DefaultCallParameters.ResponseHeaders, DefaultCallParameters.Status, DefaultCallParameters.Trailers, DefaultCallParameters.DisposeAction });

            return result;
        }

        private static object CreateAsyncUnaryCall(Type type, ISpecimenContext context)
        {
            var responseType = type.GenericTypeArguments[0];

            var method = typeof(TestCalls).GetMethod(nameof(TestCalls.AsyncUnaryCall)).MakeGenericMethod(responseType);

            var callResult = context.Resolve(responseType);

            var taskResult = typeof(Task).GetMethod(nameof(Task.FromResult)).MakeGenericMethod(responseType).Invoke(null, new[] { callResult });

            var result = method.Invoke(null, new object[] { taskResult, DefaultCallParameters.ResponseHeaders, DefaultCallParameters.Status, DefaultCallParameters.Trailers, DefaultCallParameters.DisposeAction });

            return result;
        }

        private static object CreateAsyncClientStreamingCall(Type type, ISpecimenContext context)
        {
            var requestType = type.GenericTypeArguments[0];

            var responseType = type.GenericTypeArguments[1];

            var method = typeof(TestCalls).GetMethod(nameof(TestCalls.AsyncClientStreamingCall)).MakeGenericMethod(requestType, responseType);

            var requestStream = context.Resolve(typeof(IClientStreamWriter<>).MakeGenericType(requestType));

            var callResult = context.Resolve(responseType);

            var taskResult = typeof(Task).GetMethod(nameof(Task.FromResult)).MakeGenericMethod(responseType).Invoke(null, new[] { callResult });

            var result = method.Invoke(null, new object[] { requestStream, taskResult, DefaultCallParameters.ResponseHeaders, DefaultCallParameters.Status, DefaultCallParameters.Trailers, DefaultCallParameters.DisposeAction });

            return result;
        }

        private static object CreateAsyncDuplexStreamingCall(Type type, ISpecimenContext context)
        {
            var requestType = type.GenericTypeArguments[0];

            var responseType = type.GenericTypeArguments[1];

            var method = typeof(TestCalls).GetMethod(nameof(TestCalls.AsyncDuplexStreamingCall)).MakeGenericMethod(requestType, responseType);

            var requestStream = context.Resolve(typeof(IClientStreamWriter<>).MakeGenericType(requestType));

            var responseStream = context.Resolve(typeof(IAsyncStreamReader<>).MakeGenericType(responseType));

            var result = method.Invoke(null, new object[] { requestStream, responseStream, DefaultCallParameters.ResponseHeaders, DefaultCallParameters.Status, DefaultCallParameters.Trailers, DefaultCallParameters.DisposeAction });

            return result;
        }
    }

    public class CallParameters
    {
        public CallParameters(Task<Metadata> responseHeaders, Func<Status> status, Func<Metadata> trailers, Action disposeAction)
        {
            ResponseHeaders = responseHeaders;
            Status = status;
            Trailers = trailers;
            DisposeAction = disposeAction;
        }

        public Task<Metadata> ResponseHeaders { get; }

        public Func<Status> Status { get; }

        public Func<Metadata> Trailers { get; }

        public Action DisposeAction { get; }
    }

}
