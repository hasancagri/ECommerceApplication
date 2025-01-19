using Application.Data;

using Domain.Customers;

using MediatR;

namespace Application.Customers.Commands.Create;

public record CreateCustomerCommand(string Name, string Address, string Email)
    : IRequest;


internal sealed class CreateCustomerHandler(IUnitOfWork unitOfWork, ICustomerRepository customerRepository)
    : IRequestHandler<CreateCustomerCommand>
{
    public async Task Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        var (name, address, email) = command;
        var id = Guid.NewGuid();
        Customer customer = Customer.Create(id, name, address, email);
        await customerRepository.AddAsync(customer, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}