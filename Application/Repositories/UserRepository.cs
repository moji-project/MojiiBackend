using MojiiBackend.Domain.Entities;
using MojiiBackend.Infrastructure.Database;

namespace MojiiBackend.Application.Repositories;

public class UserRepository : BaseCrudRepository<User>
{
    public UserRepository(AppDbContext context) : base(context) {}
    
    
}