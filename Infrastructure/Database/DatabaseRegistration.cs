using Microsoft.EntityFrameworkCore;

namespace MojiiBackend.Infrastructure.Database;

public static class DatabaseRegistration
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services
            .AddDbContext<AppDbContext>(options => 
                options
                    .UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                    .EnableDetailedErrors(environment.IsDevelopment())
                    .EnableSensitiveDataLogging(environment.IsDevelopment())
            );
        
        return services;
    }
}   