using Application.Customers.Commands.Create;

using Carter;

using Mapster;

using MediatR;

namespace CatalogAPI.Endpoints.Customers;

public record CreateCustomerRequest(string Name, string Address, string Email);

public class CreateCustomerEndpoint
    : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/create-customer", async (ISender sender, CreateCustomerRequest request, CancellationToken cancellationToken) =>
        {
            var command = request.Adapt<CreateCustomerCommand>();
            await sender.Send(command, cancellationToken);
            return Results.Ok();
        });
    }
}
