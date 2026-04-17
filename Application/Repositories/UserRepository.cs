using Microsoft.EntityFrameworkCore;
using MojiiBackend.Domain.Entities;
using MojiiBackend.Infrastructure.Database;

namespace MojiiBackend.Application.Repositories;

public class UserRepository(AppDbContext context)
    : BaseCrudRepository<User>(context)
{
    public async Task<List<User>> GetAllWithRelations()
    {
        return await _dbSet
            .AsNoTracking()
            .Include(u => u.Organization)
            .Include(u => u.Filiere)
            .ToListAsync();
    }

    public async Task<List<User>> GetAllUsersByOrganization(int organizationId)
    {
        return await _dbSet.AsNoTracking()
            .Where(u => u.OrganizationId == organizationId)
            .Include(u => u.Organization)
            .Include(u => u.Filiere)
            .ToListAsync();
    }
    
    public override async Task<User?> GetById(int id)
    {
        return await _dbSet
            .Include(u => u.Organization)
            .Include(u => u.Filiere)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task AffectUserToFiliere(int userId, int filiereId)
    {
        var user = await _dbSet.FindAsync(userId);
        if (user == null)
            throw new Exception("User not found");

        var filiere = await _context.Filieres.FindAsync(filiereId);
        if (filiere == null)
            throw new Exception("Filiere not found");

        user.FiliereId = filiereId;
        await _context.SaveChangesAsync();
    }
}
