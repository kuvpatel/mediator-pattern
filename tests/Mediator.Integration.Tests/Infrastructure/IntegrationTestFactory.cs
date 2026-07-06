using Mediator.Infrastructure.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Mediator.IntegrationTests.Infrastructure
{
    public class IntegrationTestFactory : WebApplicationFactory<Program>
    {
        private readonly string _connectionString;

        public IntegrationTestFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Ensure API runs in test mode
            builder.UseEnvironment("Testing");

            builder.ConfigureAppConfiguration((context, config) =>
            {
                // Override appsettings.ApiSettings.DatabaseConnectionString
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ApiSettings:DatabaseConnectionString"] = _connectionString
                });
            });
        }

        //protected override IHost CreateHost(IHostBuilder builder)
        //{
        //    // 1. Build host
        //    var host = base.CreateHost(builder);

        //    // 2. Create scope AFTER DI is fully ready
        //    using var scope = host.Services.CreateScope();

        //    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        //    // THIS is the critical fix
        //    db.Database.Migrate();


        //    return host;
        //}
    }
}
