using Microsoft.EntityFrameworkCore;
using MojiiBackend.Application.Services;
using MojiiBackend.Domain.Entities;
using MojiiBackend.Infrastructure.Database;

namespace MojiiBackend.Application.Repositories;

public class EventRepository (AppDbContext context) 
    : BaseCrudRepository<Event>(context)
{
    public async Task<List<Event>> GetAllByOrganization(int organizationId)
    {
        return await _dbSet
            .Include(e => e.Organization)
            .Include(e => e.CreatorUser)
            .Where(e => e.OrganizationId == organizationId)
            .OrderByDescending(e => e.StartDate)
            .ToListAsync();
    }

    public override async Task<List<Event>> GetAll()
    {
        return await _dbSet
            .Include(e => e.Organization)
            .Include(e => e.CreatorUser)
            .OrderByDescending(e => e.StartDate)
            .ToListAsync();
    }

    public override async Task<Event?> GetById(int id)
    {
        return await _dbSet
            .Include(e => e.Organization)
            .Include(e => e.CreatorUser)
            .Include(e => e.InterestedUsers)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task AddInterested(Event eventEntity, int userId)
    {
        var user = await _context.Users.FindAsync(userId);

        if (!eventEntity.InterestedUsers.Any(u => u.Id == userId))
        {
            eventEntity.InterestedUsers.Add(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveInterested(Event eventEntity, int userId)
    {
        var user = eventEntity.InterestedUsers.FirstOrDefault(u => u.Id == userId);
        if (user != null)
        {
            eventEntity.InterestedUsers.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}