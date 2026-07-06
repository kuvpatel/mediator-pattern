using Mediator.Domain.Entities;
using Mediator.Infrastructure.Repositories;
using Mediator.IntegrationTests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Mediator.IntegrationTests
{
    public abstract class IntegrationTestBase
    {
        protected readonly HttpClient Client;

        protected readonly IServiceProvider Services;

        private readonly SqlServerFixture _fixture;

        protected IntegrationTestBase(SqlServerFixture fixture)
        {
            _fixture = fixture;

            Client = fixture.Factory.CreateClient();
            Services = fixture.Factory.Services;
        }

        //protected async Task ResetDatabaseAsync()
        //{
        //    using var scope = Services.CreateScope();

        //    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        //    await _fixture.Respawner.ResetAsync(db);
        //}

        protected async Task<List<Customer>> SeedCustomersAsync()
        {
            using var scope = Services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            return await DatabaseSeeder.SeedCustomersAsync(db);
        }

        protected async Task<Customer> SeedCustomerAsync()
        {
            using var scope = Services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            return await DatabaseSeeder.SeedCustomerAsync(db);
        }

        protected Task ClearDatabaseAsync()
        {
            return _fixture.ResetDatabaseAsync();
        }
    }
}
