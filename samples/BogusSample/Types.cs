namespace Sample;

public class Contact
{
	public Guid Id { get; set; }
	
	public required string FirstName { get; set; }
	
	public required string LastName { get; set; }
	
	public required DateOnly DateOfBirth { get; set; }
	
	public required string Email { get; set; }
	
	public required string Username { get; set; }
}

public class Product
{
	public Guid Id { get; set; }
	
	public required string Name { get; set; }
	
	public required decimal UnitPrice { get; set; }
}

public class OrderLine
{
	public required Product Product { get; set; }
	
	public required int Quantity { get; set; }
	
	public decimal? Discount { get; set; }
}

public class Order
{
	public Guid Id { get; set; }
	
	public required Contact[] Contacts { get; set; }
	
	public required OrderLine[] OrderLines { get; set; }
}
