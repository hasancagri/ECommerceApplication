namespace Domain.Products;

public interface IProductRepository
{
    Task AddAsync(Product product, CancellationToken cancellationToken = default);
    Task<Product> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
