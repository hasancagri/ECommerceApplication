using Application.Data;

using Domain.DomainServices;

using MediatR;

namespace Application.CustomerOrders.Commands.ClearBasket;

public record ClearBasketCommand(Guid CustomerId)
    : IRequest;

internal sealed class ClearBasketHandler(IUnitOfWork unitOfWork, ICustomerOrderService customerOrderService)
    : IRequestHandler<ClearBasketCommand>
{
    public async Task Handle(ClearBasketCommand command, CancellationToken cancellationToken)
    {
        await customerOrderService.ClearBasketAsync(command.CustomerId, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}