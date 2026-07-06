using Mediator.Application.DTOs;
using Mediator.Domain.Entities;

namespace Mediator.Application.Interfaces.Persistence
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllAsync();

        Task<Customer?> GetByIdAsync(int id);

        Task<Customer> AddAsync(Customer customer);

        Task UpdateAsync(Customer customer);

        Task DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);
    }
}