using MojiiBackend.Domain.Entities;
using MojiiBackend.Infrastructure.Database;

namespace MojiiBackend.Application.Repositories;

public class NotificationRepository : BaseCrudRepository<Notification>
{
    public NotificationRepository(AppDbContext context) : base(context) {}
}
