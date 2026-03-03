using System.ComponentModel.DataAnnotations;

namespace MojiiBackend.Domain.Entities;

public class Organization : BaseEntity
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required, MaxLength(50)]
    public string City { get; set; } = string.Empty;
    
    [MaxLength(5)]
    public string? PostalCode { get; set; }
    
    [MaxLength(200)]
    public string? Address { get; set; }
    
    
    public List<Filiere> Filieres { get; set; } = [];

    public List<User> Users { get; set; } = [];
    
    public List<Event> Events { get; set; } = [];
}