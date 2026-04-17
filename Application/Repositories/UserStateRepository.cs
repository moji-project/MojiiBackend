using Microsoft.EntityFrameworkCore;
using MojiiBackend.Domain.Entities;
using MojiiBackend.Domain.Enums;
using MojiiBackend.Infrastructure.Database;

namespace MojiiBackend.Application.Repositories;

public class UserStateRepository : BaseCrudRepository<UserState>
{
    public UserStateRepository(AppDbContext context) : base(context) { }

    public async Task<List<UserState>> GetBlockedUserStatesByInitiatorId(int initiatorUserId)
    {
        return await _context.UserStates
            .AsNoTracking()
            .Where(us => us.InitiatorUserId == initiatorUserId && us.StateType == UserStateType.Blocked)
            .Include(us => us.TargetedUser)
            .ThenInclude(u => u.Organization)
            .Include(us => us.TargetedUser)
            .ThenInclude(u => u.Filiere)
            .ToListAsync();
    }
}
