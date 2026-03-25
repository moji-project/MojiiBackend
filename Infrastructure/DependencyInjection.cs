using MojiiBackend.Application;
using MojiiBackend.Application.Mappings;
using MojiiBackend.Infrastructure.Authentication;
using MojiiBackend.Infrastructure.Database;
using MojiiBackend.Infrastructure.Emailing;

namespace MojiiBackend.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services
            .AddDatabase(configuration, environment)
            .AddRepositoryServices()
            .AddApplicationServices()
            .AddScoped<EmailService>()
            .RegisterAuthentication(configuration)
            .ConfigureMapster();
        
        return services;
    }

    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}