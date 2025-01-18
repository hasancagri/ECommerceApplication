using Application.Data;

using Infrastructure.EntityFramework.Contexts;

namespace Infrastructure.EntityFramework.UnitOfWorks;

internal sealed class UnitOfWork(ApplicationContext context)
    : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await context.SaveChangesAsync(cancellationToken);
}
