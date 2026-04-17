using Microsoft.Extensions.FileProviders;
using MojiiBackend.API.Hubs;
using MojiiBackend.Application;
using MojiiBackend.Application.Mappings;
using MojiiBackend.Application.Shared;
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

        services.AddSignalR();

        return services;
    }

    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        var webRoot = app.Environment.WebRootPath ?? Path.Combine(app.Environment.ContentRootPath, "wwwroot");
        var postsUploadsFolder = Path.Combine(webRoot, "uploads", "posts");
        Directory.CreateDirectory(postsUploadsFolder);

        app.UseHttpsRedirection();

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(webRoot),
            RequestPath = ""
        });

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapHub<ChatHub>(AppConstants.ChatHubRoute);
        app.MapHub<RealtimeHub>(AppConstants.RealtimeHubRoute);
        app.MapControllers();

        return app;
    }
}
