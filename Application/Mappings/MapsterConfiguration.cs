using Mapster;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Domain.Entities;

namespace MojiiBackend.Application.Mappings;

public static class MapsterConfiguration
{
    public static IServiceCollection ConfigureMapster(this IServiceCollection services)
    {
        TypeAdapterConfig<Comment, CommentDto>
            .NewConfig()
            .Ignore("Post")
            .Ignore("Event");

        return services;
    }
}
