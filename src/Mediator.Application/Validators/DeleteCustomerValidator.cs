using FluentValidation;
using Mediator.Application.Features.Customers.Commands.CreateCustomer;

namespace Mediator.Application.Validators
{
    public class DeleteCustomerValidator : AbstractValidator<DeleteCustomerCommand>
    {
        public DeleteCustomerValidator()
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0);
        }
    }
}
