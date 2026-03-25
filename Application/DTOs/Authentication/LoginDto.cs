namespace MojiiBackend.Application.DTOs.Authentication;

public class LoginDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}