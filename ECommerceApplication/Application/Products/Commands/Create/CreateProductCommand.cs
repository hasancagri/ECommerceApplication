using Application.Data;

using Domain.Products;

using MediatR;

namespace Application.Products.Commands.Create;

public record CreateProductCommand(int Barcode, string Name, string Description, int Quantity, decimal Price)
    : IRequest;

internal sealed class CreateProductHandler(IUnitOfWork unitOfWork, IProductRepository productRepository)
    : IRequestHandler<CreateProductCommand>
{
    public async Task Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var (barcode, name, description, quantity, price) = command;
        var id = Guid.NewGuid();
        Product product = Product.Create(id, name, barcode, description, quantity, price);
        await productRepository.AddAsync(product, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
