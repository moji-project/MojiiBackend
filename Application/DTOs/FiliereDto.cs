namespace MojiiBackend.Application.DTOs;

public class FiliereDto : BaseEntityDto
{
    public string Intitule { get; set; } = string.Empty;
    public string Niveau { get; set; } = string.Empty;
    
    public int OrganizationId { get; set; }
    
    public OrganizationDto? Organization { get; set; }
}