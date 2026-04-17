namespace MojiiBackend.Application.DTOs;

public class CreateReportRequestDto
{
    // New payload
    public int Reason { get; set; }
    public int? TargetUserId { get; set; }
    public int? TargetPostId { get; set; }
    public int? TargetCommentId { get; set; }
    public string? Comment { get; set; }

    // Legacy payload compatibility
    public int? ReportedUserId { get; set; }
    public int? PostId { get; set; }
    public int? CommentId { get; set; }
    public string? Description { get; set; }
    public int? ReporterUserId { get; set; }
}

