using Application.Constants;
using Application.Services;

using Domain.Products;

using MediatR;

namespace Application.Products.EventHandlers;

internal sealed class ProductNameChangedHandler(ICacheService cacheService)
    : INotificationHandler<ProductNameChangedDomainEvent>
{
    public async Task Handle(ProductNameChangedDomainEvent notification, CancellationToken cancellationToken)
        => await cacheService.ClearAsync(CacheNames.GetProducts);
}
