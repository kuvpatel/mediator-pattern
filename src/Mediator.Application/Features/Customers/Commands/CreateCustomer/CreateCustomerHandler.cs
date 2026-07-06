using Mediator.Application.DTOs;
using Mediator.Application.Features.Customers.Commands.UpdateCustomer;
using Mediator.Application.Interfaces.Persistence;
using Mediator.Application.Mappers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Mediator.Application.Features.Customers.Commands.CreateCustomer
{
    public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, CustomerResponse>
    {
        private readonly ILogger<UpdateCustomerHandler> _logger;
        private readonly ICustomerRepository _customerRepository;

        public CreateCustomerHandler(ICustomerRepository customerRepository, ILogger<UpdateCustomerHandler> logger)
        {
            _customerRepository = customerRepository;
            _logger = logger;
        }

        public async Task<CustomerResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {

           // ArgumentNullException.ThrowIfNull(request.Customer);

            var customer = EntityMapper.Map(request.Customer);

            var customerResponse = await _customerRepository.AddAsync(customer);

            _logger.LogInformation("Created customer {Id}", customerResponse.Id);

            return EntityMapper.Map(customerResponse);

        }
    }
}