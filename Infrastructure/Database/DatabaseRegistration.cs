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
                    .EnableDetailedErrors(environment.IsDevelopment())
                    .EnableSensitiveDataLogging(environment.IsDevelopment())
            );
        
        return services;
    }

    public static void RunMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();
    }
}