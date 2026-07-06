using Mediator.Application.DTOs;
using Mediator.Application.Interfaces.Persistence;
using Mediator.Application.Mappers;
using MediatR;

namespace Mediator.Application.Features.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerCommand, CustomerResponse>
    {
        
        private readonly ICustomerRepository _customerRepository;

        public UpdateCustomerHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<CustomerResponse> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {

            var customer = await _customerRepository.GetByIdAsync(request.CustomerId);

            if (customer is null)
            {
                throw new KeyNotFoundException(
                    $"Customer with ID {request.CustomerId} was not found.");
            }

            EntityMapper.Map(request.Customer, customer, request.CustomerId);

            await _customerRepository.UpdateAsync(customer);

            return EntityMapper.Map(customer);

        }
    }
}
