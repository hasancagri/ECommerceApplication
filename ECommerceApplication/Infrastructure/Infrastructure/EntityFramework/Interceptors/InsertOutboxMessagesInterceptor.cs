using Infrastructure.EntityFramework.Contexts;

using Microsoft.EntityFrameworkCore.Diagnostics;

using Newtonsoft.Json;

namespace Infrastructure.EntityFramework.Interceptors;

public class InsertOutboxMessagesInterceptor
    : SaveChangesInterceptor
{
    private static readonly JsonSerializerSettings SerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {

        DateTime utcNow = DateTime.UtcNow;

        var applicationContext = eventData.Context as ApplicationContext;
        if (applicationContext is null)
            return await base.SavingChangesAsync(eventData, result, cancellationToken);

        var entries = applicationContext
            .ChangeTracker
            .Entries<AggregateRoot>()
            .Select(x => x.Entity);

        var domainEvents = entries
            .SelectMany(x =>
            {
                var domainEvents = x.GetDomainEvents();
                return domainEvents;
            }).ToList();

        var outboxMessages = domainEvents
            .Select(domainEvent => new OutboxMessage()
            {
                Id = Guid.NewGuid(),
                Name = domainEvent.GetType().Name,
                Content = JsonConvert.SerializeObject(domainEvent, SerializerSettings),
                CreatedOnUtc = utcNow
            });

        await applicationContext
            .Set<OutboxMessage>()
            .AddRangeAsync(outboxMessages, cancellationToken);

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
