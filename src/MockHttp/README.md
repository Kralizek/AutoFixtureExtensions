# Kralizek.AutoFixture.Extensions.MockHttp
This package is meant to provide an easy integration between [AutoFixture](https://github.com/AutoFixture/AutoFixture) and [MockHttp](https://github.com/richardszalay/mockhttp).

## Usage
You can use this extension by simply invoking the `AddMockHttp` method on the `IFixture` instance at hand.

```csharp
var fixture = new Fixture().AddMockHttp();

var testUri = fixture.Create<Uri>();
var expectedResult = fixture.Create<string>();

var handler = fixture.Freeze<MockHttpMessageHandler>();
handler.When(HttpMethod.Get, testUri.ToString()).Respond(HttpStatusCode.OK, new StringContent(expectedResult));

var sut = fixture.Create<TestService>();

var result = await sut.GetString(testUri);

Assert.That(result, Is.EqualTo(expectedResult));            
```

Alternatively, you can use a customized version of the `AutoData` attribute to improve the readibility of your unit test.

```csharp
[AttributeUsage(AttributeTargets.Method)]
public class TestAutoDataAttribute : AutoDataAttribute
{
    public TestAutoDataAttribute() : base (() => new Fixture().AddMockHttp()) { }
}

[Test, TestAutoData]
public async Task Custom_AutoData_should_work([Frozen] MockHttpMessageHandler handler, TestService sut, Uri testUri, string expectedResult)
{
    handler.When(HttpMethod.Get, testUri.ToString()).Respond(HttpStatusCode.OK, new StringContent(expectedResult));

    var result = await sut.GetString(testUri);

    Assert.That(result, Is.EqualTo(expectedResult));
}
```

## Credits
The code in this package was provided by [Andrei Ivascu](https://github.com/aivascu).
* Source: https://gist.github.com/aivascu/97a9459726bc650c8c1a4b0d6ec44ce9
* Reference: https://github.com/AutoFixture/AutoFixture/issues/1152