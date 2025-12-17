using MojiiBackend.Domain.Entities;
using MojiiBackend.Infrastructure.Database;

namespace MojiiBackend.Application.Repositories;

public class UserStateRepository: BaseCrudRepository<UserState>
{
    public UserStateRepository(AppDbContext context) : base(context) {}
    
}