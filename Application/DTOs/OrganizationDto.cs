namespace MojiiBackend.Application.DTOs;

public class OrganizationDto : BaseEntityDto
{
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string? PostalCode { get; set; }
    public string? Address { get; set; }

    public List<FiliereDto> Filieres { get; set; } = [];
}