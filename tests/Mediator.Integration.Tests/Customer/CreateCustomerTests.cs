using FluentAssertions;
using Mediator.Application.DTOs;
using Mediator.Domain.Entities;
using Mediator.Infrastructure.Repositories;
using Mediator.IntegrationTests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace Mediator.IntegrationTests.Customers
{
    [Collection("Integration Tests")]
    public class CreateCustomerTests : IntegrationTestBase
    {
        public CreateCustomerTests(SqlServerFixture fixture)
            : base(fixture)
        {
        }

        [Fact]
        public async Task CreateCustomer_ShouldReturnCreated_WhenRequestIsValid()
        {
            // Arrange
            await ClearDatabaseAsync();

            var request = new CustomerRequest
            {
                FirstName = "John",
                LastName = "Smith",
                Email = "john.smith@test.com"
            };

            // Act
            var response = await Client.PostAsJsonAsync("/api/customer", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Headers.Location.Should().NotBeNull();

            var createdCustomer =
                await response.Content.ReadFromJsonAsync<Customer>();

            createdCustomer.Should().NotBeNull();

            createdCustomer!.Id.Should().BeGreaterThan(0);
            createdCustomer.FirstName.Should().Be(request.FirstName);
            createdCustomer.LastName.Should().Be(request.LastName);
            createdCustomer.Email.Should().Be(request.Email);

            // Verify it was actually persisted
            using var scope = Services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var customer = await db.Customers.FindAsync(createdCustomer.Id);

            customer.Should().NotBeNull();
            customer!.FirstName.Should().Be(request.FirstName);
            customer.LastName.Should().Be(request.LastName);
            customer.Email.Should().Be(request.Email);
        }

        [Fact]
        public async Task CreateCustomer_ShouldReturnBadRequest_WhenRequestIsInvalid()
        {
            // Arrange
            await ClearDatabaseAsync();

            var request = new CustomerRequest
            {
                FirstName = "",
                LastName = "",
                Email = ""
            };

            // Act
            var response = await Client.PostAsJsonAsync("/api/customer", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}