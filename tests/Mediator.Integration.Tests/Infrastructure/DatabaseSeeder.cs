using Mediator.Infrastructure.Repositories;
using Mediator.Domain.Entities;

namespace Mediator.IntegrationTests.Infrastructure
{
    public static class DatabaseSeeder
    {
        public static async Task ClearDatabaseAsync(AppDbContext context)
        {
            context.Customers.RemoveRange(context.Customers);

            await context.SaveChangesAsync();
        }

        public static async Task<Customer> SeedCustomerAsync(AppDbContext context)
        {
            var customer = new Customer
            {
                FirstName = "John",
                LastName = "Smith",
                Email = "john.smith@test.com",
            };

            context.Customers.Add(customer);

            await context.SaveChangesAsync();

            return customer;
        }

        public static async Task<List<Customer>> SeedCustomersAsync(AppDbContext context, int count)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(count);

            var customers = new List<Customer>();

            for (int i = 1; i <= count; i++)
            {
                customers.Add(new Customer
                {
                    FirstName = $"First{i}",
                    LastName = $"Last{i}",
                    Email = $"customer{i}@test.com"
                });
            }

            context.Customers.AddRange(customers);

            await context.SaveChangesAsync();

            return customers;
        }
    }
}
