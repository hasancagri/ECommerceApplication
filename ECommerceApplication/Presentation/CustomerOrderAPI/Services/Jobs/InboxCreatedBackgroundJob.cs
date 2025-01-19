using CustomerOrderAPI.Contexts;
using CustomerOrderAPI.Models;

using MassTransit;

using Microsoft.EntityFrameworkCore;

using Quartz;

using Shared.IntegrationEvents;

using System.Text.Json;

namespace CustomerOrderAPI.Services.Jobs;

[DisallowConcurrentExecution]
public class InboxCreatedBackgroundJob(ApplicationDbContext applicationContext, IPublishEndpoint publishEndpoint)
    : IJob
{
    public const string AddToBasketIntegrationEventName = "AddToBasketIntegrationEvent";
    public const string ProductNameChangeIntegrationEventName = "ProductNameChangeIntegrationEvent";

    public async Task Execute(IJobExecutionContext context)
    {
        await AddToBasketAsync(applicationContext, publishEndpoint, context);
        await ProductNameChangeAsync(applicationContext, context);
    }

    private static async Task AddToBasketAsync(ApplicationDbContext applicationContext, IPublishEndpoint publishEndpoint, IJobExecutionContext context)
    {
        var outboxList = await applicationContext.OrderInboxMessages
                      .Where(x => x.Name == AddToBasketIntegrationEventName &&
                      !x.Processed)
                      .ToListAsync(context.CancellationToken);

        foreach (var outbox in outboxList)
        {
            var addToBasketIntegrationEvent = JsonSerializer
                .Deserialize<AddToBasketIntegrationEvent>(outbox.Content);

            if (addToBasketIntegrationEvent is not null)
            {
                Order order = default;
                order = await applicationContext
                   .Orders
                   .Include(x => x.OrderItems)
                   .FirstOrDefaultAsync(x => x.CustomerId == addToBasketIntegrationEvent.CustomerId, context.CancellationToken);

                if (order is null)
                {
                    order = new Order
                    {
                        Id = Guid.NewGuid(),
                        CustomerId = addToBasketIntegrationEvent.CustomerId,
                        OrderItems = [ new() {
                            ProductId = addToBasketIntegrationEvent.ProductId,
                            Price = addToBasketIntegrationEvent.Price,
                            ProductName = addToBasketIntegrationEvent.ProductName,
                            Quantity = 1,
                        }]
                    };
                    await applicationContext.Orders.AddAsync(order, context.CancellationToken);
                }
                else
                {
                    var orderItem = order.OrderItems.FirstOrDefault(x => x.ProductId == addToBasketIntegrationEvent.ProductId);
                    if (orderItem is null)
                    {
                        order.OrderItems.Add(new OrderItem
                        {
                            Id = Guid.NewGuid(),
                            ProductId = addToBasketIntegrationEvent.ProductId,
                            ProductName = addToBasketIntegrationEvent.ProductName,
                            Quantity = 1,
                        });
                        await applicationContext.OrderItems.AddAsync(orderItem, context.CancellationToken);
                    }
                    else
                    {
                        orderItem.Quantity++;
                        applicationContext.OrderItems.Update(orderItem);
                    }
                }

                outbox.Processed = true;
                await applicationContext.SaveChangesAsync(context.CancellationToken);


                SuccessfulOrderIntegrationEvent sendMessageIntegrationEvent = new()
                {
                    CustomerId = addToBasketIntegrationEvent.CustomerId,
                    ProductId = addToBasketIntegrationEvent.ProductId,
                    ProductName = addToBasketIntegrationEvent.ProductName
                };

                await publishEndpoint.Publish(sendMessageIntegrationEvent, context.CancellationToken);
            }
        }
    }

    private async Task ProductNameChangeAsync(ApplicationDbContext applicationContext, IJobExecutionContext context)
    {
        var outboxList = await applicationContext.OrderInboxMessages
                     .Where(x => x.Name == ProductNameChangeIntegrationEventName &&
                     !x.Processed)
                     .ToListAsync(context.CancellationToken);

        if (!outboxList.Any())
            return;

        foreach (var outbox in outboxList)
        {
            var productNameChangeIntegrationEvent = JsonSerializer
                .Deserialize<ProductNameChangeIntegrationEvent>(outbox.Content);
            if (productNameChangeIntegrationEvent is not null)
            {
                var orderItems = await applicationContext.OrderItems
                    .Where(x => x.ProductId == productNameChangeIntegrationEvent.ProductId)
                    .ToListAsync(context.CancellationToken);

                foreach (var orderItem in orderItems)
                {
                    orderItem.ProductName = productNameChangeIntegrationEvent.ProductName;
                    applicationContext.OrderItems.Update(orderItem);
                    outbox.Processed = true;
                    await applicationContext.SaveChangesAsync(context.CancellationToken);
                }
            }
        }
    }
}
