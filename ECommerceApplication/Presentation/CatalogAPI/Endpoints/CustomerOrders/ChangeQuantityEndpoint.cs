using Application.CustomerOrders.Commands.ChangeQuantity;

using Carter;

using Mapster;

using MediatR;

namespace CatalogAPI.Endpoints.CustomerOrders;

public record ChangeQuantityRequest(Guid CustomerId, Guid ProductId, int Quantity);


public class ChangeQuantityEndpoint
    : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/change-quantity", async (ISender sender, ChangeQuantityRequest request, CancellationToken cancellationToken) =>
        {
            var command = request.Adapt<ChangeQuantityCommand>();
            await sender.Send(command, cancellationToken);
            return Results.Ok();
        });
    }
}
