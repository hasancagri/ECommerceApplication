using Infrastructure.EntityFramework.Contexts;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework.Repositories.Customers;

internal sealed class CustomerRepository(ApplicationContext context)
    : ICustomerRepository
{
    public async Task<Customer> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await context.Customers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task AddAsync(Customer customer, CancellationToken cancellationToken = default)
        => await context.Customers.AddAsync(customer, cancellationToken);
}
