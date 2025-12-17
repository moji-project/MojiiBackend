using MojiiBackend.Application.Repositories;
using MojiiBackend.Application.Services;

namespace MojiiBackend.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
    {
        services
            .AddScoped<UserRepository>()
            .AddScoped<PostRepository>()
            .AddScoped<UserStateRepository>()
            .AddScoped<OrganizationRepository>()
            .AddScoped<FiliereRepository>()
            .AddScoped<ChannelRepository>()
            .AddScoped<CommentRepository>()
            ;
        
        return services;
    }
    
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services
            .AddScoped<UserService>()
            .AddScoped<PostService>()
            .AddScoped<OrganizationService>()
            .AddScoped<FiliereService>()
            .AddScoped<ChannelService>()
            .AddScoped<CommentService>()
            .AddScoped<UserStateService>()
            ;
        
        return services;
    }
}