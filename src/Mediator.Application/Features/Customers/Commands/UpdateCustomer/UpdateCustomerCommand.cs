using Mediator.Application.DTOs;
using MediatR;

namespace Mediator.Application.Features.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommand : IRequest<CustomerResponse>
    {
        public int CustomerId { get; set; }
        public CustomerRequest? Customer { get; set; }
    }
}