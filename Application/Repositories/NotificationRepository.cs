using Microsoft.EntityFrameworkCore;
using MojiiBackend.Domain.Entities;
using MojiiBackend.Infrastructure.Database;

namespace MojiiBackend.Application.Repositories;

public class NotificationRepository : BaseCrudRepository<Notification>
{
    public NotificationRepository(AppDbContext context) : base(context) {}

    public async Task<List<Notification>> GetForUser(int userId)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }
}
