using MassTransit;

using Shared.IntegrationEvents;

namespace IntegrationAPI.Consumers;

public class SendMailConsumer
    : IConsumer<SendMailIntegrationEvent>
{
    public async Task Consume(ConsumeContext<SendMailIntegrationEvent> context)
    {
        await File.AppendAllTextAsync("mail.txt", $"Email: {context.Message.Email}, ProductName: {context.Message.ProductName}, CustomerName: {context.Message.CustomerName}" + "\n");
    }
}
