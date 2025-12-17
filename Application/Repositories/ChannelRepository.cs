using Microsoft.EntityFrameworkCore;
using MojiiBackend.Domain.Entities;
using MojiiBackend.Infrastructure.Database;

namespace MojiiBackend.Application.Repositories;

public class ChannelRepository : BaseCrudRepository<Channel>
{
    public ChannelRepository(AppDbContext context) : base(context) {}
    
    public async Task<List<Channel>> GetAllChannelsForUser(int userId)
    {
        return await _dbSet.AsNoTracking()
            .Where(c => c.Users.Any(u => u.Id == userId))
            .Include(c => c.Users)
            .Include(c => c.Messages)
                .ThenInclude(m => m.UserSender)
            .ToListAsync();
    }
}