using FluentAssertions;
using Mediator.Infrastructure.Repositories;
using Mediator.IntegrationTests.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace Mediator.IntegrationTests.Customers
{
    [Collection("Integration Tests")]
    public class DeleteCustomerTests : IntegrationTestBase
    {
        public DeleteCustomerTests(SqlServerFixture fixture)
            : base(fixture)
        {
        }

        [Fact]
        public async Task DeleteCustomer_ShouldReturnNoContent_WhenCustomerExists()
        {
            // Arrange
            await ClearDatabaseAsync();

            var customer = await SeedCustomerAsync();

            // Act
            var response = await Client.DeleteAsync($"/api/customer/{customer.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            using var scope = Services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var deletedCustomer = await db.Customers.FindAsync(customer.Id);

            deletedCustomer.Should().BeNull();
        }

        [Fact]
        public async Task DeleteCustomer_ShouldReturnNotFound_WhenCustomerDoesNotExist()
        {
            // Arrange
            await ClearDatabaseAsync();

            const int customerId = 999999;

            // Act
            var response = await Client.DeleteAsync($"/api/customer/{customerId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var problem =
                await response.Content.ReadFromJsonAsync<ProblemDetails>();

            problem.Should().NotBeNull();
            problem!.Status.Should().Be(StatusCodes.Status404NotFound);
            problem.Detail.Should().Contain(customerId.ToString());
        }

        [Fact]
        public async Task DeleteCustomer_ShouldRemoveCustomerFromDatabase()
        {
            // Arrange
            await ClearDatabaseAsync();

            var customer = await SeedCustomerAsync();

            // Act
            await Client.DeleteAsync($"/api/customer/{customer.Id}");

            // Assert
            using var scope = Services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.Customers.FindAsync(customer.Id);

            entity.Should().BeNull();
        }

        [Fact]
        public async Task DeleteCustomer_ShouldReduceCustomerCountByOne()
        {
            // Arrange
            await ClearDatabaseAsync();

            var customers = await SeedCustomersAsync(6);

            using var scope = Services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var countBefore = db.Customers.Count();

            // Act
            await Client.DeleteAsync($"/api/customer/{customers[0].Id}");

            // Assert
            var countAfter = db.Customers.Count();

            countAfter.Should().Be(countBefore - 1);
        }
    }
}