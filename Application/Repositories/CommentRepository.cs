using Microsoft.EntityFrameworkCore;
using MojiiBackend.Domain.Entities;
using MojiiBackend.Infrastructure.Database;

namespace MojiiBackend.Application.Repositories;

public class CommentRepository : BaseCrudRepository<Comment>
{
    public CommentRepository(AppDbContext context) : base(context) {}

    public async Task<List<Comment>> GetByPostId(int postId)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(c => c.User)
            .ThenInclude(u => u.Organization)
            .Include(c => c.User)
            .ThenInclude(u => u.Filiere)
            .Where(c => c.PostId == postId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Comment>> GetByEventId(int eventId)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(c => c.User)
            .ThenInclude(u => u.Organization)
            .Include(c => c.User)
            .ThenInclude(u => u.Filiere)
            .Where(c => c.EventId == eventId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<Dictionary<int, int>> GetCommentsCountByEventIds(List<int> eventIds)
    {
        if (eventIds.Count == 0)
            return new Dictionary<int, int>();

        return await _dbSet
            .AsNoTracking()
            .Where(c => c.EventId.HasValue && eventIds.Contains(c.EventId.Value))
            .GroupBy(c => c.EventId!.Value)
            .Select(g => new { EventId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.EventId, x => x.Count);
    }

    public async Task<Comment?> GetByIdWithRelations(int commentId)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(c => c.User)
            .ThenInclude(u => u.Organization)
            .Include(c => c.User)
            .ThenInclude(u => u.Filiere)
            .FirstOrDefaultAsync(c => c.Id == commentId);
    }
}
