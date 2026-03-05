using MojiiBackend.Domain.Enums;

namespace MojiiBackend.Application.DTOs;

public class ReportDto : BaseEntityDto
{
    public int ReporterUserId { get; set; }
    public ReportReason Reason { get; set; }
    public ReportStatus Status { get; set; }
    public string? Comment { get; set; }

    public int? TargetUserId { get; set; }
    public int? TargetPostId { get; set; }
    public int? TargetCommentId { get; set; }

    public UserDto? ReporterUser { get; set; }
    public UserDto? TargetUser { get; set; }
}
