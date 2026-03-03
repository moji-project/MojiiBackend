using MojiiBackend.Domain.Entities;
using MojiiBackend.Infrastructure.Database;

namespace MojiiBackend.Application.Repositories;

public class ReportRepository : BaseCrudRepository<Report>
{
    public ReportRepository(AppDbContext context) : base(context) {}
}
