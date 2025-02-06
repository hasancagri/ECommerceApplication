using FluentValidation;

namespace Application.Customers.Commands.Create;

public class CreateCustomerValidator
    : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Email must be correct format");
    }
}
