namespace Domain.Customers;

public class Customer
    : AggregateRoot
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Address { get; private set; }
    public string Email { get; private set; }
    private Customer(Guid id, string name, string address, string email)
    {
        Id = id;
        Name = name;
        Address = address;
        Email = email;
    }

    public static Customer Create(Guid id, string name, string address, string email)
    {
        var customer = new Customer(id, name, address, email);
        customer.Raise(new CustomerCreatedDomainEvent(id, name, address, email));
        return customer;
    }
}
