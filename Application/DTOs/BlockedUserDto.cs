namespace MojiiBackend.Application.DTOs;

public class BlockedUserDto
{
    public int UserStateId { get; set; }
    public int BlockedUserId { get; set; }
    public UserDto BlockedUser { get; set; } = new();
}
