using Infrastructure.EntityFramework.Contexts;

using MassTransit;

using Microsoft.EntityFrameworkCore;

using Quartz;

using Shared.IntegrationEvents;

using System.Text.Json;

namespace Infrastructure.Services.Jobs;

[DisallowConcurrentExecution]
public class OutboxCreatedBackgroundJob(ApplicationContext applicationContext, IPublishEndpoint publishEndpoint)
    : IJob
{
    public const string AddToBasketDomainEventName = "AddToBasketDomainEvent";
    public async Task Execute(IJobExecutionContext context)
    {
        var outboxList = await applicationContext.OutboxMessages
            .Where(x => x.Name == AddToBasketDomainEventName &&
            x.ProcessedOnUtc == null)
            .ToListAsync(context.CancellationToken);

        foreach (var outbox in outboxList)
        {
            var userCreatedDomainEvent = JsonSerializer
                .Deserialize<AddToBasketDomainEvent>(outbox.Content);

            if (userCreatedDomainEvent is not null)
            {
                AddToBasketIntegrationEvent addToBasketIntegrationEvent = new()
                {
                    Id = Guid.NewGuid(),
                    CustomerId = userCreatedDomainEvent.CustomerId,
                    ProductId = userCreatedDomainEvent.ProductId
                };

                await publishEndpoint.Publish(addToBasketIntegrationEvent);
                outbox.ProcessedOnUtc = DateTime.UtcNow;
                await applicationContext.SaveChangesAsync(context.CancellationToken);
            }
        }
    }
}
