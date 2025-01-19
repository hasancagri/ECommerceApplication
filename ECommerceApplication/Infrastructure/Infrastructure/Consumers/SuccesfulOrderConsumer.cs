using Infrastructure.EntityFramework.Contexts;

using MassTransit;

using Microsoft.EntityFrameworkCore;

using Shared.IntegrationEvents;

namespace Infrastructure.Consumers;

public class SuccesfulOrderConsumer(IPublishEndpoint publishEndpoint, ApplicationContext applicationContext)
    : IConsumer<SuccessfulOrderIntegrationEvent>
{
    public async Task Consume(ConsumeContext<SuccessfulOrderIntegrationEvent> context)
    {
        var customerId = context.Message.CustomerId;
        var productId = context.Message.ProductId;
        var productName = context.Message.ProductName;

        var customer = await applicationContext
            .Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == customerId, context.CancellationToken);

        if (customer is null)
            return;

        var product = await applicationContext
           .Products
           .AsNoTracking()
           .FirstOrDefaultAsync(x => x.Id == productId, context.CancellationToken);

        if (product is null)
            return;

        SendMailIntegrationEvent sendMailIntegrationEvent = new()
        {
            CustomerName = customer.Name,
            ProductName = product.Name,
            Email = customer.Email
        };

        await publishEndpoint.Publish(sendMailIntegrationEvent, context.CancellationToken);
    }
}
