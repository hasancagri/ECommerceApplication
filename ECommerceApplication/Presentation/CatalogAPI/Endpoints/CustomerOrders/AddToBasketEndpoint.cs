using Application.CustomerOrders.Commands.AddToBasket;

using Carter;

using Mapster;

using MediatR;

namespace CatalogAPI.Endpoints.CustomerOrders;

public record AddToBasketRequest(Guid CustomerId, Guid ProductId);

public class AddToBasketEndpoint
    : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/add-to-basket", async (ISender sender, AddToBasketRequest request, CancellationToken cancellationToken) =>
        {
            var command = request.Adapt<AddToBasketCommand>();
            await sender.Send(command, cancellationToken);
            return Results.Ok();
        });
    }
}
