using System.ComponentModel.DataAnnotations;

namespace MojiiBackend.Domain.Entities;

public class Filiere : BaseEntity
{
    [Required]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    public string Intitule { get; set; } = string.Empty;
    
        
    [Required]
    public int OrganizationId { get; set; }
    public Organization Organization { get; set; } = new Organization();
    
    public List<User> Users { get; set; } = new List<User>();
}