using Application.Products.Commands.ChangeName;

using Carter;

using Mapster;

using MediatR;

namespace CatalogAPI.Endpoints.Products;

public record ChangeNameRequest(Guid Id, string Name);


public class ChangeNameEndpoint
    : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/change-name", async (ISender sender, ChangeNameRequest request, CancellationToken cancellationToken) =>
        {
            var command = request.Adapt<ChangeNameCommand>();
            await sender.Send(command, cancellationToken);
            return Results.Ok();
        });
    }
}
