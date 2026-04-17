namespace MojiiBackend.Application.DTOs;

public class RealtimeChangeDto
{
    public string Entity { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public object? Payload { get; set; }
    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
}
