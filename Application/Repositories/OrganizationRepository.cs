using MojiiBackend.Domain.Entities;
using MojiiBackend.Infrastructure.Database;

namespace MojiiBackend.Application.Repositories;

public class OrganizationRepository: BaseCrudRepository<Organization>
{
    public OrganizationRepository(AppDbContext context) : base(context) {}
    
}