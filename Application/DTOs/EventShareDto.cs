namespace MojiiBackend.Application.DTOs;

public class EventShareDto
{
    public int EventId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public string ShareUrl { get; set; } = string.Empty;
    public string ShareText { get; set; } = string.Empty;
}
