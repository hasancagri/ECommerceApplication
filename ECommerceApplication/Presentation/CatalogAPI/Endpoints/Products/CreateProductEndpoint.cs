using Application.Products.Commands.Create;

using Carter;

using Mapster;

using MediatR;

namespace CatalogAPI.Endpoints.Products;

public record CreateProductRequest(int Barcode, string Name, string Description, int Quantity, decimal Price);

public class CreateProductEndpoint
    : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/create-product", async (ISender sender, CreateProductRequest request, CancellationToken cancellationToken) =>
        {
            var command = request.Adapt<CreateProductCommand>();
            await sender.Send(command, cancellationToken);
            return Results.Ok();
        });
    }
}
