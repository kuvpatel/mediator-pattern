using Mediator.Application.DTOs;
using MediatR;

namespace Mediator.Application.Features.Customers.Commands.CreateCustomer
{
    public class DeleteCustomerCommand : IRequest
    {
        public int CustomerId { get; set; }
    }
}