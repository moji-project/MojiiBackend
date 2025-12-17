using MojiiBackend.Application;
using MojiiBackend.Application.Mappings;
using MojiiBackend.Infrastructure.Database;

namespace MojiiBackend.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services
            .AddDatabase(configuration, environment)
            .AddRepositoryServices()
            .AddApplicationServices()
            .ConfigureMapster();
        
        return services;
    }
}