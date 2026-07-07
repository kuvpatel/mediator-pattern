using FluentAssertions;
using Mediator.Application.DTOs;
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
    public class UpdateCustomerTests : IntegrationTestBase
    {
        public UpdateCustomerTests(SqlServerFixture fixture)
            : base(fixture)
        {
        }

        [Fact]
        public async Task UpdateCustomer_ShouldReturnNoContent_WhenCustomerExists()
        {
            // Arrange
            await ClearDatabaseAsync();

            var customer = await SeedCustomerAsync();

            var request = new CustomerRequest
            {
                Id = customer.Id,
                FirstName = "Peter",
                LastName = "Parker",
                Email = "peter.parker@test.com"
            };

            // Act
            var response = await Client.PutAsJsonAsync(
                $"/api/customer/{customer.Id}",
                request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            using var scope = Services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var updatedCustomer = await db.Customers.FindAsync(customer.Id);

            updatedCustomer.Should().NotBeNull();

            updatedCustomer!.FirstName.Should().Be(request.FirstName);
            updatedCustomer.LastName.Should().Be(request.LastName);
            updatedCustomer.Email.Should().Be(request.Email);
        }

        [Fact]
        public async Task UpdateCustomer_ShouldReturnNotFound_WhenCustomerDoesNotExist()
        {
            // Arrange
            await ClearDatabaseAsync();

            const int customerId = 999999;

            var request = new CustomerRequest
            {
                Id = customerId,
                FirstName = "John",
                LastName = "Smith",
                Email = "john@test.com"
            };

            // Act
            var response = await Client.PutAsJsonAsync(
                $"/api/customer/{customerId}",
                request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var problem =
                await response.Content.ReadFromJsonAsync<ProblemDetails>();

            problem.Should().NotBeNull();
            problem!.Status.Should().Be(StatusCodes.Status404NotFound);
            problem.Detail.Should().Contain(customerId.ToString());
        }

        [Fact]
        public async Task UpdateCustomer_ShouldReturnBadRequest_WhenRequestIsInvalid()
        {
            // Arrange
            await ClearDatabaseAsync();

            var customer = await SeedCustomerAsync();

            var request = new CustomerRequest
            {
                Id = customer.Id,
                FirstName = "",
                LastName = "",
                Email = ""
            };

            // Act
            var response = await Client.PutAsJsonAsync(
                $"/api/customer/{customer.Id}",
                request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateCustomer_ShouldPersistChangesToDatabase()
        {
            // Arrange
            await ClearDatabaseAsync();

            var customer = await SeedCustomerAsync();

            var request = new CustomerRequest
            {
                Id = customer.Id,
                FirstName = "Bruce",
                LastName = "Wayne",
                Email = "bruce.wayne@test.com"
            };

            // Act
            await Client.PutAsJsonAsync(
                $"/api/customer/{customer.Id}",
                request);

            // Assert

            using var scope = Services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.Customers.FindAsync(customer.Id);

            entity.Should().NotBeNull();

            entity!.FirstName.Should().Be("Bruce");
            entity.LastName.Should().Be("Wayne");
            entity.Email.Should().Be("bruce.wayne@test.com");
        }
    }
}