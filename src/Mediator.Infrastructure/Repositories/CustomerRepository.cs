using Mediator.Application.Common;
using Mediator.Application.Interfaces.Persistence;
using Mediator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Mediator.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;
        private readonly AppSettings _config;

        public CustomerRepository(IOptions<AppSettings> appsettings, AppDbContext context)
        {
            _context = context;

        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _context.Customers
            .AsNoTracking()
            .ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _context.Customers
             .AsNoTracking()
             .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Customer> AddAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task UpdateAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
                return;

            _context.Customers.Remove(customer);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Customers
                .AnyAsync(c => c.Id == id);
        }
    }
}
