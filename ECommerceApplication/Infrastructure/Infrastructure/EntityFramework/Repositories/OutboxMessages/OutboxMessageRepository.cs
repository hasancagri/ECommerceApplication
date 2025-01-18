using Infrastructure.EntityFramework.Contexts;

namespace Infrastructure.EntityFramework.Repositories.OutboxMessages;

internal sealed class OutboxMessageRepository(ApplicationContext context)
    : IOutboxMessageRepository
{
    public async Task AddAsync(OutboxMessage message, CancellationToken cancellationToken = default)
        => await context.OutboxMessages.AddAsync(message, cancellationToken);
}
