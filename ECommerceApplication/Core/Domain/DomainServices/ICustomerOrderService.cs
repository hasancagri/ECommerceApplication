namespace Domain.DomainServices;

public interface ICustomerOrderService
{
    Task AddToBasketAsync(Guid customerId, Guid productId, CancellationToken cancellationToken = default);
    Task ChangeQuantityAsync(Guid customerId, Guid productId, int quantity, CancellationToken cancellationToken = default);
    Task ClearBasketAsync(Guid customerId, CancellationToken cancellationToken = default);
}
