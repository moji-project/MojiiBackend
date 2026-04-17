using Microsoft.EntityFrameworkCore;
using MojiiBackend.Domain.Entities;
using MojiiBackend.Infrastructure.Database;

namespace MojiiBackend.Application.Repositories;

public class MessageRepository : BaseCrudRepository<Message>
{
    public MessageRepository(AppDbContext context) : base(context) { }

    public async Task<List<Message>> GetByChannelId(int channelId)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(m => m.ChannelId == channelId)
            .Include(m => m.UserSender)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();
    }

    public async Task<Message?> GetByIdWithSender(int messageId)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(m => m.UserSender)
            .FirstOrDefaultAsync(m => m.Id == messageId);
    }
}
