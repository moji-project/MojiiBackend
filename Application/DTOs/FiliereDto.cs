namespace MojiiBackend.Application.DTOs;

public class FiliereDto : BaseEntityDto
{
    public string Intitule { get; set; } = string.Empty;
    public string Niveau { get; set; } = string.Empty;
    public string? AnneeScolaire { get; set; }
    
    public int OrganizationId { get; set; }
   
    // Ca crée une dépendance circulaire (donc l app crashe)
    // public OrganizationDto? Organization { get; set; }
}

public class FiliereWithInfosDto : FiliereDto
{
    public int NbOfStudents { get; set; } = 0;
    public int NbOfMojiis { get; set; } = 0;
}