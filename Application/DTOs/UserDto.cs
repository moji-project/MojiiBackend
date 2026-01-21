using MojiiBackend.Domain.Enums;

namespace MojiiBackend.Application.DTOs;

public class UserDto : BaseEntityDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Biography { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public bool IsConnected { get; set; }
    public DateTime? LastConnectionDate { get; set; }
}