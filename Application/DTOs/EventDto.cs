namespace MojiiBackend.Application.DTOs;

public class EventDto : BaseEntityDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Location { get; set; }
    public string? Address { get; set; }
    public string? DateLabel { get; set; }
    public string? MonthLabel { get; set; }
    public string? DayLabel { get; set; }
    public int DefaultInterestedCount { get; set; } = 0;
    public DateTime StartDate { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsPublished { get; set; } = false;
    public bool IsInterestedByCurrentUser { get; set; } = false;
    public int InterestedCount { get; set; } = 0;
    public bool IsLikedByCurrentUser { get; set; } = false;
    public int LikesCount { get; set; } = 0;
    public int CommentsCount { get; set; } = 0;

    public int OrganizationId { get; set; }
    public int CreatorUserId { get; set; }

    public OrganizationDto? Organization { get; set; }
    public UserDto? CreatorUser { get; set; }

    public List<UserDto>? InterestedUsers { get; set; } = [];
}
