using Application.Data;

using Domain.DomainServices;

using MediatR;

namespace Application.CustomerOrders.Commands.ChangeQuantity;

public record ChangeQuantityCommand(Guid CustomerId, Guid ProductId, int Quantity)
    : IRequest;

internal sealed class ChangeQuantityHandler(IUnitOfWork unitOfWork, ICustomerOrderService customerOrderService)
    : IRequestHandler<ChangeQuantityCommand>
{
    public async Task Handle(ChangeQuantityCommand command, CancellationToken cancellationToken)
    {
        var (customerId, productId, quantity) = command;
        await customerOrderService.ChangeQuantityAsync(customerId, productId, quantity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}