using System.Text.Json;
using System.Text.Json.Serialization;
using AutoFixture;
using Bogus;
using Sample;

var fixture = new Fixture();

fixture.CustomizeWithBogus<Contact>(b => b
    .RuleFor(c => c.Id, (f, c) => f.Random.Guid())
    .RuleFor(c => c.FirstName, (f, c) => f.Person.FirstName)
    .RuleFor(c => c.LastName, (f, c) => f.Person.LastName)
    .RuleFor(c => c.DateOfBirth, (f, c) => f.Date.PastDateOnly(60, DateOnly.FromDateTime(DateTime.Now.AddYears(-18))))
    .RuleFor(c => c.Email, (f, c) => f.Internet.Email(c.FirstName, c.LastName))
    .RuleFor(c => c.Username, (f, c) => f.Internet.UserName(c.FirstName, c.LastName).ToLower())
);

fixture.CustomizeWithBogus<Product>(b => b
    .RuleFor(c => c.Id, (f, c) => f.Random.Guid())
    .RuleFor(c => c.Name, (f, c) => f.Commerce.ProductName())
    .RuleFor(c => c.UnitPrice, (f, c) => decimal.Parse(f.Commerce.Price(10, 100)))
);

fixture.CustomizeWithBogus<OrderLine>(b => b
    .RuleFor(c => c.Product, () => fixture.Create<Product>())
    .RuleFor(c => c.Discount, (f, c) => f.Random.Decimal(0.1m, c.Product.UnitPrice).OrNull(f, 0.60f))
    .RuleFor(c => c.Quantity, (f, c) => f.Random.Int(1, 10))
);

fixture.Customize<DateOnly>(o => o.FromFactory((DateTime dt) => DateOnly.FromDateTime(dt)));

var order = fixture.Create<Order>();

order.PrintAsJson();