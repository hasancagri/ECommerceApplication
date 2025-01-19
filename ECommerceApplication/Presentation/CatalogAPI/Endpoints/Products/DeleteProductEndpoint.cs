using Application.Products.Commands.Delete;

using Carter;

using Mapster;

using MediatR;

namespace CatalogAPI.Endpoints.Products;

public record DeleteProductRequest(Guid ProductId);

public class DeleteProductEndpoint
    : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/delete-product", async (ISender sender, DeleteProductRequest request, CancellationToken cancellationToken) =>
        {
            var command = request.Adapt<DeleteProductCommand>();
            await sender.Send(command, cancellationToken);
            return Results.Ok();
        });
    }
}
