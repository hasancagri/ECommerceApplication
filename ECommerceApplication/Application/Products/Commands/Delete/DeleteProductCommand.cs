using MediatR;

namespace Application.Products.Commands.Delete;

public record DeleteProductCommand(Guid ProductId)
    : IRequest;

internal sealed class DeleteProductHandler
    : IRequestHandler<DeleteProductCommand>
{
    public Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}