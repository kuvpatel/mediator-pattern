using Mediator.Application.DTOs;
using MediatR;

namespace Mediator.Application.Features.Customers.Queries.GetCustomers
{
    public class GetCustomersQuery : IRequest<IEnumerable<CustomerResponse?>>
    {
        
    }
}