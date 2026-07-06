using Mediator.Application.DTOs;
using MediatR;

namespace Mediator.Application.Features.Customers.Queries.GetCustomerById
{
    public class GetCustomerQuery : IRequest<CustomerResponse?>
    {
        public int CustomerId { get; set; }
    }
}