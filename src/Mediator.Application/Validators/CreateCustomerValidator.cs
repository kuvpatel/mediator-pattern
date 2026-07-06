using FluentValidation;
using Mediator.Application.Features.Customers.Commands.CreateCustomer;

namespace Mediator.Application.Validators
{
    public class CreateCustomerValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerValidator()
        {
            RuleFor(x => x.Customer)
                .NotNull()
                .SetValidator(new CustomerRequestValidator()!);
        }
    }
}
