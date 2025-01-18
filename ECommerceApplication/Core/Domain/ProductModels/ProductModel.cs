
namespace Domain.ProductModels;


public class ProductModel
{
    public Guid Id { get; set; }
    public int Barcode { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}