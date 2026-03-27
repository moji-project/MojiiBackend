namespace MojiiBackend.Application.DTOs;

public class EventDto : BaseEntityDto
{
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsPublished { get; set; } = false;

    public int OrganizationId { get; set; }
    public int CreatorUserId { get; set; }

    public OrganizationDto? Organization { get; set; }
    public UserDto? CreatorUser { get; set; }

    public List<UserDto>? InterestedUsers { get; set; } = [];
}
