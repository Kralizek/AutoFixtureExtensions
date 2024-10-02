# Kralizek.AutoFixture.Extensions.AspNetCore.RazorPages

This package can be used to configure AutoFixture so that it can create instances of classes inheriting from `PageModel`.

## Usage

You can use this extension by simply invoking the `AddRazorPages` method on the `IFixture` instance at hand.

```csharp
var fixture = new Fixture().AddRazorPages();

var page = fixture.Create<IndexPage>();

var result = await page.OnGetAsync(cancellationToken: default);

Assert.That(result, Is.InstanceOf<PageResult>());
```
