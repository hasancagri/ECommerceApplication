using Application.Data;

using Domain.DomainServices;

using MediatR;

namespace Application.CustomerOrders.Commands.AddToBasket;

internal record AddToBasketCommand(Guid CustomerId, Guid ProductId)
    : IRequest;

internal sealed class AddToBasketHandler(IUnitOfWork unitOfWork, ICustomerOrderService customerOrderService)
    : IRequestHandler<AddToBasketCommand>
{
    public async Task Handle(AddToBasketCommand command, CancellationToken cancellationToken)
    {
        var (customerId, productId) = command;
        await customerOrderService.AddToBasketAsync(customerId, productId, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}