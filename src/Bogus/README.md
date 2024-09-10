# Kralizek.AutoFixture.Extensions.Bogus

This package is meant to provide an easy way to leverage Bogus fake definition with AutoFixture.

## Usage

You can use this extension by simply invoking the `CustomizeWithBogus<T>` method on the `IFixture` instance at hand.

```csharp
public class Contact
{
    public Guid Id { get; set; }
    
    public required string FirstName { get; set; }
    
    public required string LastName { get; set; }
    
    public required DateOnly DateOfBirth { get; set; }
    
    public required string Email { get; set; }
    
    public required string Username { get; set; }
}

var fixture = new Fixture();

fixture.CustomizeWithBogus<Contact>(b => b
    .RuleFor(c => c.Id, (f, c) => f.Random.Guid())
    .RuleFor(c => c.FirstName, (f, c) => f.Person.FirstName)
    .RuleFor(c => c.LastName, (f, c) => f.Person.LastName)
    .RuleFor(c => c.DateOfBirth, (f, c) => f.Date.PastDateOnly(60, DateOnly.FromDateTime(DateTime.Now.AddYears(-18))))
    .RuleFor(c => c.Email, (f, c) => f.Internet.Email(c.FirstName, c.LastName))
    .RuleFor(c => c.Username, (f, c) => f.Internet.UserName(c.FirstName, c.LastName).ToLower())
);

var person = fixture.Create<Person>();
```
