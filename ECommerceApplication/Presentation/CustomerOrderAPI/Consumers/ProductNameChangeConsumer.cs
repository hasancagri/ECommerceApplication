using CustomerOrderAPI.Contexts;
using CustomerOrderAPI.Models;

using MassTransit;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

using Shared.IntegrationEvents;

namespace CustomerOrderAPI.Consumers;

public class ProductNameChangeConsumer(ApplicationDbContext applicationContext)
    : IConsumer<ProductNameChangeIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductNameChangeIntegrationEvent> context)
    {
        if (await applicationContext.OrderInboxMessages.AnyAsync(x => x.IdempotentToken == context.Message.Id))
            return;
        OrderInboxMessage orderInboxMessage = new()
        {
            Id = Guid.NewGuid(),
            Content = JsonConvert.SerializeObject(context.Message),
            IdempotentToken = context.Message.Id,
            Name = nameof(ProductNameChangeIntegrationEvent),
        };

        await applicationContext.OrderInboxMessages.AddAsync(orderInboxMessage);
        await applicationContext.SaveChangesAsync();
    }
}
