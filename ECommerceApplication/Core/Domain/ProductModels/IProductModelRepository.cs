namespace Domain.ProductModels;

public interface IProductModelRepository
{
    Task<List<ProductModel>> GetAllAsync(CancellationToken cancellationToken = default);
}
