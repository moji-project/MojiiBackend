namespace MojiiBackend.Application.DTOs;

public class BaseEntityDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}