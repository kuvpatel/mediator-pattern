using Mediator.Application.Interfaces.Persistence;
using MediatR;

namespace Mediator.Application.Features.Customers.Commands.CreateCustomer
{
    public class DeleteCustomerHandler : IRequestHandler<DeleteCustomerCommand>
    {
        
        private readonly ICustomerRepository _customerRepository;

        public DeleteCustomerHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task Handle(DeleteCustomerCommand request,CancellationToken cancellationToken)
        {

            var customer = await _customerRepository.GetByIdAsync(request.CustomerId);

            if (customer is null)
            {
                throw new KeyNotFoundException(
                    $"Customer with ID {request.CustomerId} was not found.");
            }

            await _customerRepository.DeleteAsync(request.CustomerId);
        }
    }
}