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
        
        TypeAdapterConfig<Event, EventDto>.NewConfig()
            .Map(dest => dest.Organization, src => (OrganizationDto?) null)
            .Map(dest => dest.CreatorUser, src => (UserDto?) null)
            .Map(dest => dest.InterestedUsers, src => (List<UserDto>?) null);

        return services;
    }
}
