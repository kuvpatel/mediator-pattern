using Mediator.Application.DTOs;
using MediatR;

namespace Mediator.Application.Features.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommand : IRequest<CustomerResponse>
    {
        public CustomerRequest? Customer { get; set; }
    }
}