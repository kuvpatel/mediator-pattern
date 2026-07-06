using FluentValidation;
using Mediator.Application.Features.Customers.Queries.GetCustomerById;


namespace Mediator.Application.Validators
{
    public class GetCustomersValidator
     : AbstractValidator<GetCustomerQuery>
    {
        public GetCustomersValidator()
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0);
            
        }
    }
}
