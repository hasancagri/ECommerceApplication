using Domain.ProductModels;

using Infrastructure.EntityFramework.Contexts;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework.Repositories.ProductModels;

internal sealed class ProductModelRepository(ApplicationContext context)
    : IProductModelRepository
{
    public Task<List<ProductModel>> GetAllAsync(CancellationToken cancellationToken = default)
        => context.Products
        .AsNoTracking()
        .Select(x => new ProductModel
        {
            Id = x.Id,
            Description = x.Description,
            Price = x.Price,
            Quantity = x.Quantity,
        }).ToListAsync(cancellationToken);
}
