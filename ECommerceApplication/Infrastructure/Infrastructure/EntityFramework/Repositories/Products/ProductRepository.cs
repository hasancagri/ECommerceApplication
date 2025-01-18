using Infrastructure.EntityFramework.Contexts;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework.Repositories.Products;

internal sealed class ProductRepository(ApplicationContext context)
    : IProductRepository
{
    public async Task<Product> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await context.Products.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
        => await context.Products.AddAsync(product, cancellationToken);
}
