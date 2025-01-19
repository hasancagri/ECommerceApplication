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
    public const string ProductNameChangedDomainEventName = "ProductNameChangedDomainEvent";
    public async Task Execute(IJobExecutionContext context)
    {
        await AddToBasket(applicationContext, publishEndpoint, context);
        await ProductNameChanged(applicationContext, publishEndpoint, context);
    }

    private static async Task AddToBasket(ApplicationContext applicationContext, IPublishEndpoint publishEndpoint, IJobExecutionContext context)
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
                    ProductId = userCreatedDomainEvent.ProductId,
                    ProductName = userCreatedDomainEvent.ProductName,
                    Price = userCreatedDomainEvent.Price
                };

                await publishEndpoint.Publish(addToBasketIntegrationEvent);
                outbox.ProcessedOnUtc = DateTime.UtcNow;
                await applicationContext.SaveChangesAsync(context.CancellationToken);
            }
        }
    }

    private static async Task ProductNameChanged(ApplicationContext applicationContext, IPublishEndpoint publishEndpoint, IJobExecutionContext context)
    {
        var outboxList = await applicationContext.OutboxMessages
                    .Where(x => x.Name == ProductNameChangedDomainEventName &&
                    x.ProcessedOnUtc == null)
                    .ToListAsync(context.CancellationToken);

        foreach (var outbox in outboxList)
        {
            var userCreatedDomainEvent = JsonSerializer
                .Deserialize<ProductNameChangedDomainEvent>(outbox.Content);

            if (userCreatedDomainEvent is not null)
            {
                ProductNameChangeIntegrationEvent productNameChangeIntegrationEvent = new()
                {
                    Id = Guid.NewGuid(),
                    ProductId = userCreatedDomainEvent.Id,
                    ProductName = userCreatedDomainEvent.Name
                };

                await publishEndpoint.Publish(productNameChangeIntegrationEvent);
                outbox.ProcessedOnUtc = DateTime.UtcNow;
                await applicationContext.SaveChangesAsync(context.CancellationToken);
            }
        }
    }
}
