using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
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
        
    }
}
