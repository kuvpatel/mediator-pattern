using FluentValidation;
using Mediator.Application.Features.Customers.Commands.UpdateCustomer;

namespace Mediator.Application.Validators
{
    public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerValidator()
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0);

            RuleFor(x => x.Customer)
                .NotNull()
                .SetValidator(new CustomerRequestValidator()!);
        }
    }
}