using Application.CustomerOrders.Commands.ClearBasket;

using Carter;

using Mapster;

using MediatR;

namespace CatalogAPI.Endpoints.CustomerOrders;

public record ClearBasketRequest(Guid CustomerId);

public class ClearBasketEndpoint
    : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/clear-basket", async (ISender sender, ClearBasketRequest request, CancellationToken cancellationToken) =>
        {
            var command = request.Adapt<ClearBasketCommand>();
            await sender.Send(command, cancellationToken);
            return Results.Ok();
        });
    }
}
