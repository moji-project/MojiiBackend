using Microsoft.AspNetCore.Identity;
using MojiiBackend.Application.Repositories;
using MojiiBackend.Application.Services;
using MojiiBackend.Domain.Entities;

namespace MojiiBackend.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
    {
        services
            .AddScoped<PostRepository>()
            .AddScoped<UserStateRepository>()
            .AddScoped<OrganizationRepository>()
            .AddScoped<FiliereRepository>()
            .AddScoped<ChannelRepository>()
            .AddScoped<CommentRepository>()
            .AddScoped<RefreshTokenRepository>()
            .AddScoped<EventRepository>()
            .AddScoped<NotificationRepository>()
            .AddScoped<ReportRepository>();
        
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
            .AddScoped<AuthService>()
            .AddScoped<TokenService>()
            .AddScoped<EventService>()
            .AddScoped<NotificationService>()
            .AddScoped<ReportService>();
        
        return services;
    }
}