# Kralizek.AutoFixture.Extensions.AspNetCore.WebApplicationFactory

This package is meant to provide an easy integration between [AutoFixture](https://github.com/AutoFixture/AutoFixture) and [Microsoft.AspNetCore.Mvc.Testing](https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.Testing) to create integration tests for ASP.NET Core web applications.

## Basic usage
You can use this extension by simply invoking the `AddWebApplicationFactorySupport` method on the `IFixture` instance at hand.

```csharp
var fixture = new Fixture().AddWebApplicationFactorySupport<TestWebSite.Startup>();

var message = fixture.Create<string>();

var http = fixture.Create<HttpClient>();

var response = await http.PostAsync("/", new FormUrlEncodedContent(new Dictionary<string, string> { ["message"] = message }));

response.EnsureSuccessStatusCode();

var responseContent = await response.Content.ReadAsStringAsync();

Assert.That(responseContent, Is.EqualTo(message));
```

## Usage with `AutoData`

Alternatively, you can use a customized version of the `AutoData` attribute to improve the readability of your unit tests.

```csharp
[AttributeUsage(AttributeTargets.Method)]
public class TestAutoDataAttribute : AutoDataAttribute
{
    public TestAutoDataAttribute() : base(() => new Fixture().AddWebApplicationFactorySupport<TestWebSite.Startup>()) { }
}

[Test, TestAutoData]
public async Task Should_work_with_custom_auto_data_attribute(HttpClient http, string message)
{
    var response = await http.PostAsync("/", new FormUrlEncodedContent(new Dictionary<string, string> { ["message"] = message }));

    response.EnsureSuccessStatusCode();

    var responseContent = await response.Content.ReadAsStringAsync();

    Assert.That(responseContent, Is.EqualTo(message));
}
```

The `HttpClient` received as a parameter is already configured to target the in-memory instance of the web application under test.

## Customizing the tested application

The `WebApplicationFactory` utility offered by [Microsoft.AspNetCore.Mvc.Testing](https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.Testing) gives developers the possibility to customize the application to test.

A simple example is altering the service registrations to provide fakes instead of real dependencies.

This library allows the same workflow.

```csharp
public interface IService
{
    string Echo(string message);
}

[AttributeUsage(AttributeTargets.Method)]
public class TestAutoDataAttribute : AutoDataAttribute
{
    public TestAutoDataAttribute() : base(CreateFixture) { }

    private static IFixture CreateFixture()
    {
        var fixture = new Fixture();

        fixture.Customize(new AutoMoqCustomization
        {
            ConfigureMembers = true,
            GenerateDelegates = true
        });

        fixture.Register<IService>(() => Mock.Of<MyTestServiceImpl>());

        fixture.AddWebApplicationFactorySupport<TestWebSite.Startup>(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddTransient<IService>(_ => fixture.Create<IService>());
            });
        });

        return fixture;
    }
}

[Test, TestAutoData]
public async Task Dependencies_are_overridden(HttpClient http, IService service)
{
    Mock.Get(service).Setup(p => p.Echo(It.IsAny<string>())).Returns((string str) => str);

    Assume.That(service, Is.InstanceOf<MyTestServiceImpl>());

    var response = await http.PostAsync("/", new FormUrlEncodedContent(new Dictionary<string, string> { ["message"] = message }));

    Mock.Get(service).Verify(p => p.Echo(It.IsAny<string>()), Times.Once());
}
```
