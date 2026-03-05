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
}