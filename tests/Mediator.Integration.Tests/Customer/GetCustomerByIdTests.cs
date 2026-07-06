using FluentAssertions;
using Mediator.Domain.Entities;
using Mediator.IntegrationTests.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;


namespace Mediator.IntegrationTests.Customers
{
    [Collection("Integration Tests")]
    public class GetCustomerByIdTests : IntegrationTestBase
    {
        public GetCustomerByIdTests(SqlServerFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task GetCustomerById_ShouldReturnCustomer_WhenCustomerExists()
        {
            // Arrange
            await ClearDatabaseAsync();
            var customer = await SeedCustomerAsync();

            // Act
            var response = await Client.GetAsync($"/api/customer/{customer.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result =
                await response.Content.ReadFromJsonAsync<Customer>();

            result.Should().NotBeNull();

            result!.Id.Should().Be(customer.Id);
            result.FirstName.Should().Be(customer.FirstName);
            result.LastName.Should().Be(customer.LastName);
            result.Email.Should().Be(customer.Email);
        }

        [Fact]
        public async Task GetCustomerById_ShouldReturnNotFound_WhenCustomerDoesNotExist()
        {
            // Arrange
            await ClearDatabaseAsync();

            const int nonExistentCustomerId = 999999;

            // Act
            var response = await Client.GetAsync($"/api/customer/{nonExistentCustomerId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

            problem.Should().NotBeNull();
            problem!.Status.Should().Be(StatusCodes.Status404NotFound);
            problem.Title.Should().Be("Resource not found"); // Adjust to match your GlobalExceptionHandler
            problem.Detail.Should().Contain(nonExistentCustomerId.ToString());
        }

    }
}
