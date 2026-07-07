using FluentAssertions;
using Mediator.Domain.Entities;
using Mediator.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;

namespace Mediator.IntegrationTests.Customers
{
    [Collection("Integration Tests")]
    public class GetCustomersTests : IntegrationTestBase
    {
        public GetCustomersTests(SqlServerFixture fixture)
            : base(fixture)
        {
        }

        [Fact]
        public async Task GetCustomers_ShouldReturnAllCustomers_WhenCustomersExist()
        {
            // Arrange
            await ClearDatabaseAsync();

            var seededCustomers = await SeedCustomersAsync(3);

            // Act
            var response = await Client.GetAsync("/api/customer");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var customers =
                await response.Content.ReadFromJsonAsync<List<Customer>>();

            customers.Should().NotBeNull();
            customers.Should().HaveCount(3);

            customers!
                .Select(c => c.Email)
                .Should()
                .BeEquivalentTo(
                    seededCustomers.Select(c => c.Email));
        }

        [Fact]
        public async Task GetCustomers_ShouldReturnEmptyList_WhenNoCustomersExist()
        {
            // Arrange
            await ClearDatabaseAsync();

            // Act
            var response = await Client.GetAsync("/api/customer");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var customers =
                await response.Content.ReadFromJsonAsync<List<Customer>>();

            customers.Should().NotBeNull();
            customers.Should().BeEmpty();
        }

        [Fact]
        public async Task GetCustomers_ShouldReturnExpectedCustomerData()
        {
            // Arrange
            await ClearDatabaseAsync();

            var seededCustomers = await SeedCustomersAsync(5);

            // Act
            var response = await Client.GetAsync("/api/customer");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var customers =
                await response.Content.ReadFromJsonAsync<List<Customer>>();

            customers.Should().NotBeNull();
            customers.Should().HaveCount(seededCustomers.Count);

            foreach (var seeded in seededCustomers)
            {
                var actual = customers!
                    .Single(c => c.Id == seeded.Id);

                actual.FirstName.Should().Be(seeded.FirstName);
                actual.LastName.Should().Be(seeded.LastName);
                actual.Email.Should().Be(seeded.Email);
            }
        }
    }
}