using Mapster;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Repositories;
using MojiiBackend.Domain.Entities;
using MojiiBackend.Domain.Enums;
using MojiiBackend.Infrastructure.Emailing;

namespace MojiiBackend.Application.Services;

public class ReportService(
    ReportRepository reportRepository,
    UserRepository userRepository,
    PostRepository postRepository,
    CommentRepository commentRepository,
    ICurrentUserService currentUserService,
    EmailService emailService)
{
    private const string ReportNotificationEmail = "lukas.bouhlel@ynov.com";

    public async Task<List<ReportDto>> GetAllReports()
    {
        var reports = await reportRepository.GetAll();
        return reports.Select(MapReportToDto).ToList();
    }

    public async Task<ReportDto?> GetReportById(int id)
    {
        var report = await reportRepository.GetById(id);
        return report is null ? null : MapReportToDto(report);
    }

    public async Task<ReportDto> CreateReport(CreateReportRequestDto createReportRequestDto)
    {
        if (createReportRequestDto.Reason is < 0 or > 4)
            throw new ArgumentException("Invalid reason value. Allowed values: 0 (InapropriateContent), 1 (Spam), 2 (Harassment), 3 (FakeInformation), 4 (Other).");

        var targetUserId = createReportRequestDto.TargetUserId ?? createReportRequestDto.ReportedUserId;
        var targetPostId = createReportRequestDto.TargetPostId ?? createReportRequestDto.PostId;
        var targetCommentId = createReportRequestDto.TargetCommentId ?? createReportRequestDto.CommentId;

        if (targetUserId is null && targetPostId is null && targetCommentId is null)
            throw new ArgumentException("At least one target is required: targetUserId, targetPostId or targetCommentId.");

        if (targetUserId.HasValue && targetUserId.Value <= 0 ||
            targetPostId.HasValue && targetPostId.Value <= 0 ||
            targetCommentId.HasValue && targetCommentId.Value <= 0)
        {
            throw new ArgumentException("Target IDs must be positive integers.");
        }

        if (targetUserId.HasValue && !await userRepository.Exists(targetUserId.Value))
            throw new ArgumentException("Target user not found.");

        if (targetPostId.HasValue && !await postRepository.Exists(targetPostId.Value))
            throw new ArgumentException("Target post not found.");

        if (targetCommentId.HasValue && !await commentRepository.Exists(targetCommentId.Value))
            throw new ArgumentException("Target comment not found.");

        var normalizedComment = string.IsNullOrWhiteSpace(createReportRequestDto.Comment)
            ? createReportRequestDto.Description?.Trim()
            : createReportRequestDto.Comment.Trim();

        if (!string.IsNullOrWhiteSpace(normalizedComment) && normalizedComment.Length > 500)
            throw new ArgumentException("Comment cannot exceed 500 characters.");

        var report = new Report
        {
            ReporterUserId = currentUserService.GetUserId(),
            Reason = (ReportReason)createReportRequestDto.Reason,
            Status = ReportStatus.Open,
            Comment = string.IsNullOrWhiteSpace(normalizedComment) ? null : normalizedComment,
            TargetUserId = targetUserId,
            TargetPostId = targetPostId,
            TargetCommentId = targetCommentId
        };

        await reportRepository.Create(report);

        try
        {
            var reporterUser = await userRepository.GetById(report.ReporterUserId);

            await emailService.SendReportNotificationAsync(
                ReportNotificationEmail,
                report.Id,
                report.ReporterUserId,
                reporterUser?.Email,
                reporterUser?.FirstName,
                reporterUser?.LastName,
                report.Reason.ToString(),
                report.Status.ToString(),
                report.TargetUserId,
                report.TargetPostId,
                report.TargetCommentId,
                report.Comment);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[REPORT EMAIL ERROR] : {ex.Message}");
        }

        return MapReportToDto(report);
    }

    public async Task UpdateReport(ReportDto reportDto)
    {
        Report report = reportDto.Adapt<Report>();
        await reportRepository.Update(report);
    }

    public async Task DeleteReport(int id)
    {
        await reportRepository.Delete(id);
    }

    private static ReportDto MapReportToDto(Report report)
    {
        return new ReportDto
        {
            Id = report.Id,
            CreatedAt = report.CreatedAt,
            UpdatedAt = report.UpdatedAt,
            ReporterUserId = report.ReporterUserId,
            Reason = report.Reason,
            Status = report.Status,
            Comment = report.Comment,
            TargetUserId = report.TargetUserId,
            TargetPostId = report.TargetPostId,
            TargetCommentId = report.TargetCommentId
        };
    }
}
