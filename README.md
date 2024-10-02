# AutoFixture Extensions

This repository contains a set of small extension packages to make the life of AutoFixture users easier.

## Packages

The following packages are available:

* **[Kralizek.AutoFixture.Extensions.MockHttp](src/MockHttp)**<br/>
  An integration between AutoFixture and [MockHttp](https://github.com/richardszalay/mockhttp) to make HTTP testing easier.

* **[Kralizek.AutoFixture.Extensions.AspNetCore.WebApplicationFactory](src/AspNetCore.WebApplicationFactory)**<br/>
  An integration between AutoFixture and [Microsoft.AspNetCore.Mvc.Testing](https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.Testing) to create integration tests for ASP.NET Core web applications.

* **[Kralizek.AutoFixture.Extensions.Grpc](src/Grpc)**<br/>
  An integration between AutoFixture and [Grpc.Core.Testing](https://www.nuget.org/packages/Grpc.Core.Testing/) to test GRPC services and components consuming GRPC clients.

* **[Kralizek.AutoFixture.Extensions.ServiceProvider](src/ServiceProvider)**<br/>
  An integration between AutoFixture and [Microsoft.Extensions.DependencyInjection](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection) to resolve registered services.

* **[Kralizek.AutoFixture.Extensions.Bogus](src/Bogus)**<br/>
  A library that allows the usage of [Bogus](https://github.com/bchavez/Bogus) to generate fake data in the AutoFixture context.

* **[Kralizek.AutoFixture.Extensions.AspNetCore.RazorPages](src/AspNetCore.RazorPages)**<br/>
  A library that enables the possibility for AutoFixture to create classes inheriting from `PageModel`.

## License

The content of this repository is licensed under the [MIT license](https://github.com/Kralizek/AutoFixtureExtensions/blob/master/LICENSE.txt).
