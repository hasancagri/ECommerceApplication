using Domain.ProductModels;

using MediatR;

namespace Application.Products.Queries.GetAll;

public record GetAllProductsQuery()
    : IRequest<List<ProductModel>>;


internal sealed class GetAllProductsHandler(IProductModelRepository productModelRepository)
    : IRequestHandler<GetAllProductsQuery, List<ProductModel>>
{
    public async Task<List<ProductModel>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
        => await productModelRepository.GetAllAsync(cancellationToken);
}