using Mediator.Infrastructure.Repositories;
using System.Data.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;
using Respawn;


namespace Mediator.IntegrationTests.Infrastructure
{
    public sealed class SqlServerFixture : IAsyncLifetime
    {
        private readonly MsSqlContainer _container =
        new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPassword("Your_strong_Password123!")
            .Build();

        public Respawner _respawner = null;

        public IntegrationTestFactory Factory { get; private set; } = null!;


        public async Task InitializeAsync()
        {
            await _container.StartAsync();

            var connectionString = _container
                .GetConnectionString()
                .Replace("Database=master", "Database=SimpleCustomerDb");

            Factory = new IntegrationTestFactory(connectionString);

            // Force the host to start
            _ = Factory.Server;

            // Apply migrations
            using var scope = Factory.Services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await db.Database.MigrateAsync();

            // Create Respawner
            DbConnection connection = db.Database.GetDbConnection();

            await connection.OpenAsync();

            _respawner = await Respawner.CreateAsync(connection, new RespawnerOptions
            {
                DbAdapter = DbAdapter.SqlServer,
                TablesToIgnore =
                [
                    "__EFMigrationsHistory"
                ]
            });

            await connection.CloseAsync();
        }

        public async Task ResetDatabaseAsync()
        {
            using var scope = Factory.Services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var connection = db.Database.GetDbConnection();

            await connection.OpenAsync();

            await _respawner.ResetAsync(connection);

            await connection.CloseAsync();
        }

        public async Task DisposeAsync()
        {
            Factory.Dispose();
            await _container.DisposeAsync();
        }
    }
}
