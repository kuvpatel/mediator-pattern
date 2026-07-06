using Mediator.Application.DTOs;
using Mediator.Application.Interfaces.Persistence;
using Mediator.Application.Mappers;
using MediatR;

namespace Mediator.Application.Features.Customers.Queries.GetCustomerById
{
    public class GetCustomerHandler : IRequestHandler<GetCustomerQuery, CustomerResponse?>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomerHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<CustomerResponse?> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId);


            if (customer is null)
            {
                throw new KeyNotFoundException(
                    $"Customer with ID {request.CustomerId} was not found.");
            }

            var customerResponse = EntityMapper.Map(customer);

            return customerResponse;
        }
    }
}