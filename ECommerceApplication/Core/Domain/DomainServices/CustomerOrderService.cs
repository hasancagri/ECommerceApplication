
using Domain.Customers;
using Domain.Outbox;
using Domain.Products;

using Newtonsoft.Json;

namespace Domain.DomainServices;

public sealed class CustomerOrderService(IProductRepository productRepository,
                                    ICustomerRepository customerRepository,
                                    IOutboxMessageRepository outboxMessageRepository)
    : ICustomerOrderService
{
    private static readonly JsonSerializerSettings SerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };

    public async Task AddToBasketAsync(Guid customerId, Guid productId, CancellationToken cancellationToken = default)
    {
        DateTime utcNow = DateTime.UtcNow;

        Product product = await productRepository.GetByIdAsync(productId, cancellationToken);
        if (product is null)
            throw new Exception($"Product not found by Id={productId}");

        Customer customer = await customerRepository.GetByIdAsync(customerId, cancellationToken);
        if (customer is null)
            throw new Exception($"Customer not found by Id={customerId}");

        AddToBasketDomainEvent domainEvent = new(customerId, productId, product.Name, product.Price);
        OutboxMessage outboxMessage = new()
        {
            Id = Guid.NewGuid(),
            Name = domainEvent.GetType().Name,
            CreatedOnUtc = utcNow,
            Content = JsonConvert.SerializeObject(domainEvent, SerializerSettings)
        };

        await outboxMessageRepository.AddAsync(outboxMessage, cancellationToken);
    }

    public async Task ChangeQuantityAsync(Guid customerId, Guid productId, int quantity, CancellationToken cancellationToken = default)
    {
        DateTime utcNow = DateTime.UtcNow;

        Product product = await productRepository.GetByIdAsync(productId, cancellationToken);
        if (product is null)
            throw new Exception($"Product not found by Id={productId}");

        Customer customer = await customerRepository.GetByIdAsync(customerId, cancellationToken);
        if (customer is null)
            throw new Exception($"Customer not found by Id={customerId}");

        ChangeQuantityDomainEvent domainEvent = new(customerId, productId, quantity);
        OutboxMessage outboxMessage = new()
        {
            Id = Guid.NewGuid(),
            Name = domainEvent.GetType().Name,
            CreatedOnUtc = utcNow,
            Content = JsonConvert.SerializeObject(domainEvent, SerializerSettings)
        };

        await outboxMessageRepository.AddAsync(outboxMessage, cancellationToken);
    }

    public async Task ClearBasketAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        DateTime utcNow = DateTime.UtcNow;

        Customer customer = await customerRepository.GetByIdAsync(customerId, cancellationToken);
        if (customer is null)
            throw new Exception($"Customer not found by Id={customerId}");

        ClearBasketDomainEvent domainEvent = new(customerId);
        OutboxMessage outboxMessage = new()
        {
            Id = Guid.NewGuid(),
            Name = domainEvent.GetType().Name,
            CreatedOnUtc = utcNow,
            Content = JsonConvert.SerializeObject(domainEvent, SerializerSettings)
        };

        await outboxMessageRepository.AddAsync(outboxMessage, cancellationToken);
    }
}
