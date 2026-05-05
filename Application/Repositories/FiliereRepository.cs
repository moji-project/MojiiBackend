using Microsoft.EntityFrameworkCore;
using MojiiBackend.Domain.Entities;
using MojiiBackend.Infrastructure.Database;

namespace MojiiBackend.Application.Repositories;

public class FiliereRepository : BaseCrudRepository<Filiere>
{
    public FiliereRepository(AppDbContext context) : base(context) {}
    
    public async Task<List<Filiere>> GetAllByOrganization(int organizationId)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(f => f.OrganizationId == organizationId)
            .ToListAsync();
    }

    public async Task<int> GetNbOfStudentsForFiliere(int filiereId)
    {
        return await _context.Users
            .CountAsync(u => u.FiliereId == filiereId);
    }
    
    public async Task<int> GetNbOfMojiisForFiliere(int filiereId)
    {
        return await _context.Posts
            .CountAsync(p => _context.Users
                .Any(u => u.Id == p.UserId && u.FiliereId == filiereId));
    }

    public async Task<List<User>> GetUsersByFiliere(int filiereId)
    {
        return await _context.Users
            .AsNoTracking()
            .Where(u => u.FiliereId == filiereId)
            .ToListAsync();
    }
}