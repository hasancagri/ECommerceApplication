using Application.Data;

using Domain.Products;

using MediatR;

namespace Application.Products.Commands.ChangeName;

public record ChangeNameCommand(Guid Id, string Name)
    : IRequest;

internal class ChangeNameHandler(IUnitOfWork unitOfWork, IProductRepository productRepository)
    : IRequestHandler<ChangeNameCommand>
{
    public async Task Handle(ChangeNameCommand command, CancellationToken cancellationToken)
    {
        var (id, name) = command;
        var product = await productRepository.GetByIdAsync(id, cancellationToken);
        if (product is null)
            throw new Exception("Product not found.");

        product.ChangeName(name);
        productRepository.Update(product);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
