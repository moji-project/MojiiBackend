using Microsoft.EntityFrameworkCore;
using MojiiBackend.Domain.Entities;
using MojiiBackend.Domain.Enums;
using MojiiBackend.Infrastructure.Database;

namespace MojiiBackend.Application.Repositories;

public class OrganizationRepository : BaseCrudRepository<Organization>
{
    public OrganizationRepository(AppDbContext context) : base(context) {}

    public async Task<List<User>> GetStudentsOfOrganizationOrderByCreationDate(int organizationId)
    {
        return await _context.Users
            .AsNoTracking()
            .Where(u => u.OrganizationId == organizationId)
            .Include(u => u.Organization)
            .Include(u => u.Filiere)
            .Include(u => u.CreatedPosts)
            .OrderByDescending(u => u.CreatedAt)
            .ToListAsync();
    }

    public async Task<int> GetNbOfStudents(int organizationId)
    {
        return await _context.Users
            .CountAsync(u => u.OrganizationId == organizationId);
    }

    public async Task<int> GetNbOfFilieres(int organizationId)
    {
        return await _context.Filieres
            .CountAsync(f => f.OrganizationId == organizationId);
    }

    public async Task<int> GetNbOfPostsThisWeek(int organizationId)
    {
        var now = DateTime.UtcNow;
        var daysSinceMonday = ((int)now.DayOfWeek + 6) % 7;
        var startOfWeek = now.Date.AddDays(-daysSinceMonday);
        var endOfWeek = startOfWeek.AddDays(7);

        return await _context.Posts
            .CountAsync(p =>
                p.CreatedAt >= startOfWeek &&
                p.CreatedAt < endOfWeek &&
                p.User.OrganizationId == organizationId);
    }

    public async Task<int> GetNbOfOpenReports(int organizationId)
    {
        return await _context.Reports
            .CountAsync(r =>
                r.Status != ReportStatus.Closed &&
                (
                    r.ReporterUser.OrganizationId == organizationId ||
                    (r.TargetUser != null && r.TargetUser.OrganizationId == organizationId) ||
                    (r.TargetPost != null && r.TargetPost.User.OrganizationId == organizationId) ||
                    (r.TargetComment != null && r.TargetComment.User.OrganizationId == organizationId) ||
                    (r.TargetComment != null && r.TargetComment.Post != null && r.TargetComment.Post.User.OrganizationId == organizationId) ||
                    (r.TargetComment != null && r.TargetComment.Event != null && r.TargetComment.Event.OrganizationId == organizationId)
                ));
    }
}
