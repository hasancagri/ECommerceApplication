using MassTransit;

using Shared.IntegrationEvents;

namespace IntegrationAPI.Consumers;

public class SendMailConsumer
    : IConsumer<SendMailIntegrationEvent>
{
    public async Task Consume(ConsumeContext<SendMailIntegrationEvent> context)
    {
        var email = context.Message.Email;
        var productName = context.Message.ProductName;
        var customerName = context.Message.CustomerName;
        await File.AppendAllTextAsync("mail.txt", $"Email: {email}, ProductName: {productName}, CustomerName: {customerName}" + "\n");
    }
}
