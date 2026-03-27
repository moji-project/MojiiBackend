using MojiiBackend.Domain.Enums;

namespace MojiiBackend.Application.DTOs;

public class UserDto : BaseEntityDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Biography { get; set; }
    public string? ProfilePicUrl { get; set; }
    public bool IsConnected { get; set; }
    public DateTime? LastConnectionDate { get; set; }
    
    public int OrganizationId { get; set; } 
    public OrganizationDto? Organization { get; set; }
    
    public int FiliereId { get; set; }
    public FiliereDto? Filiere { get; set; }

    public List<PostDto>? CreatedPosts { get; set; } = [];
    public List<UserStateDto>? UserStates { get; set; } = [];
}