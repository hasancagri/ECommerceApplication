using Application.Data;

using Domain.Products;

using MediatR;

namespace Application.Products.Commands.Delete;

public record DeleteProductCommand(Guid ProductId)
    : IRequest;

internal sealed class DeleteProductHandler(IUnitOfWork unitOfWork, IProductRepository productRepository)
    : IRequestHandler<DeleteProductCommand>
{
    public Task Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}