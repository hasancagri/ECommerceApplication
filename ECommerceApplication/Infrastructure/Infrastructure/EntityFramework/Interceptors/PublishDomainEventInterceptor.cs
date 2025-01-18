using Infrastructure.EntityFramework.Contexts;

using MediatR;

using Microsoft.EntityFrameworkCore.Diagnostics;


namespace Infrastructure.EntityFramework.Interceptors;

public class PublishDomainEventInterceptor(IPublisher publisher)
    : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
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

        entries
            .ToList()
            .ForEach(x => x.Clear());

        foreach (var domainEvent in domainEvents)
            await publisher.Publish(domainEvent, cancellationToken);

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
