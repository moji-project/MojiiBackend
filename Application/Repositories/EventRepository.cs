using MojiiBackend.Domain.Entities;
using MojiiBackend.Infrastructure.Database;

namespace MojiiBackend.Application.Repositories;

public class EventRepository: BaseCrudRepository<Event>
{
    public EventRepository(AppDbContext context) : base(context) {}
}