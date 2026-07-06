using Mediator.Application.DTOs;
using Mediator.Application.Interfaces.Persistence;
using Mediator.Application.Mappers;
using MediatR;

namespace Mediator.Application.Features.Customers.Queries.GetCustomers
{
    public class GetCustomersHandler : IRequestHandler<GetCustomersQuery, IEnumerable<CustomerResponse?>>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomersHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<CustomerResponse?>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            var customers = await _customerRepository.GetAllAsync();

            if (customers is null)
            {
                throw new KeyNotFoundException(
                    $"Customers were not found.");
            }

            var customerResponses = EntityMapper.Map(customers);

            return customerResponses;
        }
    }
}