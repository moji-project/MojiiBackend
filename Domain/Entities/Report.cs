using System.ComponentModel.DataAnnotations;
using MojiiBackend.Domain.Enums;

namespace MojiiBackend.Domain.Entities;

public class Report : BaseEntity
{
    public int ReporterUserId { get; set; }
    public User ReporterUser { get; set; } = null!;
    
    public ReportReason Reason { get; set; }
    public ReportStatus Status { get; set; }
    
    [MaxLength(500)]
    public string? Comment { get; set; }
    
    public int? TargetUserId { get; set; }
    public User? TargetUser { get; set; }

    public int? TargetPostId { get; set; }
    public Post? TargetPost { get; set; }

    public int? TargetCommentId { get; set; }
    public Comment? TargetComment { get; set; }
}
