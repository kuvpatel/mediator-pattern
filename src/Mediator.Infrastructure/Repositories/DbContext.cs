using Mediator.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Mediator.Infrastructure.Repositories
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers => Set<Customer>();
    }
}
