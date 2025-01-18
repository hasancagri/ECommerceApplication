using Application.Constants;
using Application.Services;

using Domain.Products;

using MediatR;

namespace Application.Products.EventHandlers;

internal sealed class ProductCreatedHandler(ICacheService cacheService)
    : INotificationHandler<ProductCreatedDomainEvent>
{
    public async Task Handle(ProductCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await cacheService.ClearAsync(CacheNames.GetProducts);
    }
}
