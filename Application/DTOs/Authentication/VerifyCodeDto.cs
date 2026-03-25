namespace MojiiBackend.Application.DTOs.Authentication;

public class VerifyCodeDto
{
    public string Email { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}