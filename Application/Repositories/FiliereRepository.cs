using MojiiBackend.Domain.Entities;
using MojiiBackend.Infrastructure.Database;

namespace MojiiBackend.Application.Repositories;

public class FiliereRepository : BaseCrudRepository<Filiere>
{
    public FiliereRepository(AppDbContext context) : base(context) {}
    
}