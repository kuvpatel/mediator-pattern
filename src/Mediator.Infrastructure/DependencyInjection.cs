using Mediator.Application.Interfaces.Persistence;
using Mediator.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Mediator.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            return services;
        }
    }
}