namespace Domain.Products;

public class Product
    : AggregateRoot
{
    public Guid Id { get; private set; }
    public string Name { get; set; }
    public int Barcode { get; private set; }
    public string Description { get; private set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }

    private Product(Guid id, string name, int barcode, string description, int quantity, decimal price)
    {
        Id = id;
        Name = name;
        Barcode = barcode;
        Description = description;
        Quantity = quantity;
        Price = price;
    }

    public static Product Create(Guid id, string name, int barcode, string description, int quantity, decimal price)
    {
        var product = new Product(id, name, barcode, description, quantity, price);
        product.Raise(new ProductCreatedDomainEvent(id, barcode, description, quantity, price));
        return product;
    }

    public void ChangeName(string name)
    {
        Name = name;
        Raise(new ProductNameChangedDomainEvent(Id, name));
    }
}
