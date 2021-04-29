using System;
using System.Threading.Tasks;
using AutoFixture.Kernel;
using Grpc.Core;
using Grpc.Core.Testing;

namespace Kralizek.AutoFixture.Extensions.Internal
{
    public class GrpcAsyncCallSpecimenBuilder : ISpecimenBuilder
    {
        private readonly CallParameters _callParameters;
        
        public GrpcAsyncCallSpecimenBuilder(CallParameters callParameters)
        {
            _callParameters = callParameters ?? throw new ArgumentNullException(nameof(callParameters));
        }

        public object Create(object request, ISpecimenContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return request switch
            {
                null => throw new ArgumentNullException(nameof(request)),
                Type { IsGenericType: true } type when type.GetGenericTypeDefinition() == typeof(AsyncServerStreamingCall<>) => CreateAsyncServerStreamingCall(type, context, _callParameters),
                Type { IsGenericType: true } type when type.GetGenericTypeDefinition() == typeof(AsyncUnaryCall<>) => CreateAsyncUnaryCall(type, context, _callParameters),
                Type { IsGenericType: true } type when type.GetGenericTypeDefinition() == typeof(AsyncClientStreamingCall<,>) => CreateAsyncClientStreamingCall(type, context, _callParameters),
                Type { IsGenericType: true } type when type.GetGenericTypeDefinition() == typeof(AsyncDuplexStreamingCall<,>) => CreateAsyncDuplexStreamingCall(type, context, _callParameters),
                _ => new NoSpecimen()
            };
        }

        private static object CreateAsyncServerStreamingCall(Type type, ISpecimenContext context, CallParameters callParameters)
        {
            var typeArgument = type.GenericTypeArguments[0];

            var asyncStreamReaderType = typeof(IAsyncStreamReader<>).MakeGenericType(typeArgument);

            var responseStream = context.Resolve(asyncStreamReaderType);

            var method = typeof(TestCalls).GetMethod(nameof(TestCalls.AsyncServerStreamingCall)).MakeGenericMethod(typeArgument);

            var result = method.Invoke(null, new[] { responseStream, callParameters.ResponseHeaders, callParameters.Status, callParameters.Trailers, callParameters.DisposeAction });

            return result;
        }

        private static object CreateAsyncUnaryCall(Type type, ISpecimenContext context, CallParameters callParameters)
        {
            var responseType = type.GenericTypeArguments[0];

            var method = typeof(TestCalls).GetMethod(nameof(TestCalls.AsyncUnaryCall)).MakeGenericMethod(responseType);

            var callResult = context.Resolve(responseType);

            var taskResult = typeof(Task).GetMethod(nameof(Task.FromResult)).MakeGenericMethod(responseType).Invoke(null, new[] { callResult });

            var result = method.Invoke(null, new object[] { taskResult, callParameters.ResponseHeaders, callParameters.Status, callParameters.Trailers, callParameters.DisposeAction });

            return result;
        }

        private static object CreateAsyncClientStreamingCall(Type type, ISpecimenContext context, CallParameters callParameters)
        {
            var requestType = type.GenericTypeArguments[0];

            var responseType = type.GenericTypeArguments[1];

            var method = typeof(TestCalls).GetMethod(nameof(TestCalls.AsyncClientStreamingCall)).MakeGenericMethod(requestType, responseType);

            var requestStream = context.Resolve(typeof(IClientStreamWriter<>).MakeGenericType(requestType));

            var callResult = context.Resolve(responseType);

            var taskResult = typeof(Task).GetMethod(nameof(Task.FromResult)).MakeGenericMethod(responseType).Invoke(null, new[] { callResult });

            var result = method.Invoke(null, new object[] { requestStream, taskResult, callParameters.ResponseHeaders, callParameters.Status, callParameters.Trailers, callParameters.DisposeAction });

            return result;
        }

        private static object CreateAsyncDuplexStreamingCall(Type type, ISpecimenContext context, CallParameters callParameters)
        {
            var requestType = type.GenericTypeArguments[0];

            var responseType = type.GenericTypeArguments[1];

            var method = typeof(TestCalls).GetMethod(nameof(TestCalls.AsyncDuplexStreamingCall)).MakeGenericMethod(requestType, responseType);

            var requestStream = context.Resolve(typeof(IClientStreamWriter<>).MakeGenericType(requestType));

            var responseStream = context.Resolve(typeof(IAsyncStreamReader<>).MakeGenericType(responseType));

            var result = method.Invoke(null, new object[] { requestStream, responseStream, callParameters.ResponseHeaders, callParameters.Status, callParameters.Trailers, callParameters.DisposeAction });

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
