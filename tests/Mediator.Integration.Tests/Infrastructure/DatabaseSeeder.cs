using Mediator.Infrastructure.Repositories;
using Mediator.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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

        public static async Task<List<Customer>> SeedCustomersAsync(AppDbContext context)
        {
            var customers = new List<Customer>
            {
                new Customer
                {
                    FirstName = "John",
                    LastName = "Smith",
                    Email = "john@test.com",
                },
                new Customer
                {
                    FirstName = "Jane",
                    LastName = "Doe",
                    Email = "jane@test.com",

                },
                new Customer
                {
                    FirstName = "Bob",
                    LastName = "Johnson",
                    Email = "bob@test.com",
                }
            };

            context.Customers.AddRange(customers);

            await context.SaveChangesAsync();

            return customers;
        }
    }
}
