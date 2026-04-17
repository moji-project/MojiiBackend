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

    public async Task<List<Channel>> GetDirectChannelsForUser(int userId, string directPrefix)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(c => c.Name.StartsWith(directPrefix) && c.Users.Any(u => u.Id == userId))
            .Include(c => c.Users)
            .Include(c => c.Messages)
                .ThenInclude(m => m.UserSender)
            .ToListAsync();
    }

    public async Task<Channel?> GetDirectChannelByName(string directChannelName)
    {
        return await _dbSet
            .Include(c => c.Users)
            .Include(c => c.Messages)
                .ThenInclude(m => m.UserSender)
            .FirstOrDefaultAsync(c => c.Name == directChannelName);
    }

    public async Task<Channel?> GetByIdWithUsersAndMessages(int channelId)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(c => c.Users)
            .Include(c => c.Messages)
                .ThenInclude(m => m.UserSender)
            .FirstOrDefaultAsync(c => c.Id == channelId);
    }

    public async Task<bool> IsUserInChannel(int channelId, int userId)
    {
        return await _dbSet
            .AsNoTracking()
            .AnyAsync(c => c.Id == channelId && c.Users.Any(u => u.Id == userId));
    }

    public async Task<Channel> CreateDirectChannel(string directChannelName, int user1Id, int user2Id)
    {
        var firstUser = await _context.Users.FindAsync(user1Id);
        var secondUser = await _context.Users.FindAsync(user2Id);

        if (firstUser == null || secondUser == null)
            throw new ArgumentException("One or both users were not found.");

        var channel = new Channel
        {
            Name = directChannelName,
            Users = [firstUser, secondUser]
        };

        await _dbSet.AddAsync(channel);
        await _context.SaveChangesAsync();

        return channel;
    }
}
