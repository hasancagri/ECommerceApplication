namespace Domain.Customers;

public class Customer
    : AggregateRoot
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Address { get; private set; }

    private Customer(Guid id, string name, string address)
    {
        Id = id;
        Name = name;
        Address = address;
    }

    public static Customer Create(Guid id, string name, string address)
    {
        var customer = new Customer(id, name, address);
        customer.Raise(new CustomerCreatedDomainEvent(id, name, address));
        return customer;
    }
}
