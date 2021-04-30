# Kralizek.AutoFixture.Extensions.Grpc

This package is meant to provide an easy integration between [AutoFixture](https://github.com/AutoFixture/AutoFixture) and [Grpc.Core.Testing](https://www.nuget.org/packages/Grpc.Core.Testing/) to test GRPC services and components consuming GRPC clients.

## Installation

To use the facilities offered by this package, simply install it using the CLI or Visual Studio.

```sh
$ dotnet add package Kralizek.AutoFixture.Extensions.Grpc
```

Once the package is installed, decorate your `IFixture` instance by using the `AddGrpcSupport` method.

```csharp
var fixture = new Fixture().AddGrpcSupport();
```

The `AddGrpcSupport` will register customizations that instruct AutoFixture how to create instances of the test doubles available in [Grpc.Core.Testing](https://www.nuget.org/packages/Grpc.Core.Testing/).

## Testing a component consuming a GRPC client

Testing a service consuming a GRPC client requires to properly mock the responses it returns.

This package customizes AutoFixture so that it can create instances of the call responses and use them to create parameterized tests.

Let's consider a component `Service` that implements the following interface

```csharp
public interface IService
{
    Task<string> SayHello(string name);

    Task<string> StreamHellos(IEnumerable<string> names);

    IAsyncEnumerable<string> ReceiveHelloStream(string name);

    IAsyncEnumerable<string> HelloDuplex(IEnumerable<string> names);
}
```

Notice that the examples below are written using `Moq` and `AutoMoq`, a library that glues `Moq` and `AutoFixture` together.

### Mocking asynchronous unary calls

The snippet below shows how to mock a unary call. The response is built with an instance of the response type.

```csharp
[Test, ClientAutoData]
public async Task Service_uses_internal_client_for_unary_requests([Frozen] Greeter.GreeterClient client, Service sut, [Frozen] HelloReply reply, AsyncUnaryCall<HelloReply> responseCall, string name)
{
    Mock.Get(client).Setup(p => p.SayHelloUnaryAsync(It.IsAny<HelloRequest>(), null, null, default)).Returns(responseCall);

    var response = await sut.SayHello(name);

    Assert.That(response, Is.EqualTo(reply.Message));
}
```

### Mocking asynchronous calls with the server streaming the response

The snippet below shows how to mock a call where the server responds with a stream of items. The response is built with a fixed set of instance of the response type.

```csharp
[Test, ClientAutoData]
public async Task Service_uses_internal_client_for_server_streaming([Frozen] Greeter.GreeterClient client, Service sut, [Frozen] HelloReply reply, AsyncServerStreamingCall<HelloReply> responseCall, string name)
{
    Mock.Get(client).Setup(p => p.SayHelloServerStream(It.IsAny<HelloRequest>(), null, null, default)).Returns(responseCall);

    var response = await sut.ReceiveHelloStream(name).ToListAsync();

    Assert.That(response, Contains.Item(reply.Message));
}
```

### Mocking asynchronous calls with parameters streamed by the client

The snippet below shows how to mock a call where the client is used to stream requests to the remote service.

```csharp
[Test, ClientAutoData]
public async Task Service_uses_internal_client_for_client_streaming_requests([Frozen] Greeter.GreeterClient client, Service sut, [Frozen] HelloReply reply, AsyncClientStreamingCall<HelloRequest, HelloReply> responseCall, string[] names)
{
    Mock.Get(client).Setup(p => p.SayHelloClientStream(null, null, default)).Returns(responseCall);

    var response = await sut.StreamHellos(names);

    Assert.That(response, Is.EqualTo(reply.Message));
}
```

### Mocking asynchronous duplex streaming calls

The snippet below shows how to mock a call where the client is used to stream requests to the remote service. The remote service will stream the response back to the client.

```csharp
[Test, ClientAutoData]
public async Task Service_uses_internal_client_for_duplex_streaming([Frozen] Greeter.GreeterClient client, Service sut, [Frozen] HelloReply reply, AsyncDuplexStreamingCall<HelloRequest, HelloReply> responseCall, string[] names)
{
    Mock.Get(client).Setup(p => p.SayHelloDuplex(null, null, default)).Returns(responseCall);

    var response = await sut.HelloDuplex(names).ToListAsync();

    Assert.That(response, Contains.Item(reply.Message));
}
```

### Setup

The snippets above are built based on `ClientAutoData` built as follows:

```csharp
[AttributeUsage(AttributeTargets.Method)]
public class ClientAutoDataAttribute : AutoDataAttribute
{
    public ClientAutoDataAttribute() : base(CreateFixture) { }

    private static IFixture CreateFixture()
    {
        var fixture = new Fixture();

        fixture.AddGrpcSupport();

        fixture.Customize(new AutoMoqCustomization
        {
            ConfigureMembers = true,
            GenerateDelegates = true
        });

        fixture.Register(Mock.Of<Greeter.GreeterClient>);

        return fixture;
    }
}
```

To be noted:
- `AddGrpcSupport` to instruct `fixture` with the GRPC specific test doubles
- `AutoMoq` is used to help `AutoFixture` create mocks when an interface is requested
- `fixture.Register(Mock.Of<Greeter.GreeterClient>);` instructs AutoFixture to provide a mock when requested for a client instead of an instance. This is required because `Greeter.GreeterClient` is not `abstract`. 

## Testing GRPC services

Testing GRPC services requires being able to handle the different parameters needed to invoke a service method.

### Testing asynchronous unary operations

The snippet below shows how to invoke an unary operation and assert the result of the call.

```csharp
[Test, ServiceAutoData]
public async Task Service_greets_sender_unary(GreeterService sut, string name, ServerCallContext context)
{
    var response = await sut.SayHelloUnary(new HelloRequest { Name = name }, context);

    Assert.That(response.Message, Does.EndWith(name));
}
```

### Testing asynchronous operations with the server streaming the response

The snippet below shows how to invoke an operation and assert the result of the call when the service streams the response.

```csharp
[Test, ServiceAutoData]
public async Task Service_greets_sender_server_stream(GreeterService sut, ServerCallContext context, IServerStreamWriter<HelloReply> responseStream, string name)
{
    await sut.SayHelloServerStream(new HelloRequest { Name = name }, responseStream, context);

    Mock.Get(responseStream).Verify(p => p.WriteAsync(It.Is<HelloReply>(r => r.Message.EndsWith(name))), Times.Exactly(10));
}
```

### Testing asynchronous operations with parameters streamed by the client

The snippet below shows how to invoke an operation and assert the result of the call when the client streams the parameters.

```csharp
[Test, ServiceAutoData]
public async Task Service_greets_sender_client_stream(GreeterService sut, ServerCallContext context, [Frozen] IEnumerable<HelloRequest> requests, IAsyncStreamReader<HelloRequest> requestStream)
{
    var response = await sut.SayHelloClientStream(requestStream, context);

    foreach (var req in requests)
    {
        StringAssert.Contains(req.Name, response.Message);
    }
}
```

### Testing asynchronous duplex streaming operations

The snippet below shows how to invoke an operation and assert the result of the call when the client streams the parameters and the service streams the response.

```csharp
[Test, ServiceAutoData]
public async Task Service_greets_sender_duplex(GreeterService sut, ServerCallContext context, [Frozen] IEnumerable<HelloRequest> requests, IAsyncStreamReader<HelloRequest> requestStream, IServerStreamWriter<HelloReply> responseStream)
{
    await sut.SayHelloDuplex(requestStream, responseStream, context);

    foreach (var req in requests)
    {
        Mock.Get(responseStream).Verify(p => p.WriteAsync(It.Is<HelloReply>(r => r.Message.EndsWith(req.Name))), Times.Once());
    }
}
```

### Setup

The snippets above are built based on `ServiceAutoData` built as follows:

```csharp
[AttributeUsage(AttributeTargets.Method)]
public class ServiceAutoDataAttribute : AutoDataAttribute
{
    public ServiceAutoDataAttribute() : base(CreateFixture) { }

    private static IFixture CreateFixture()
    {
        var fixture = new Fixture();

        fixture.AddGrpcSupport();

        fixture.Customize(new AutoMoqCustomization
        {
            ConfigureMembers = true,
            GenerateDelegates = true
        });

        return fixture;
    }
}
```

To be noted:
- `AddGrpcSupport` to instruct `fixture` with the GRPC specific test doubles
- `AutoMoq` is used to help `AutoFixture` create mocks when an interface is requested.