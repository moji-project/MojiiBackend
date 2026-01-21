using System.ComponentModel.DataAnnotations;

namespace MojiiBackend.Domain.Entities;

public class Filiere : BaseEntity
{
    [Required, MaxLength(100)]
    public string Intitule { get; set; } = string.Empty;
 
    [Required, MaxLength(6)]
    public string Niveau { get; set; } = string.Empty;
    
        
    [Required]
    public int OrganizationId { get; set; }
    public Organization Organization { get; set; } = new Organization();
    
    public List<User> Users { get; set; } = [];
}