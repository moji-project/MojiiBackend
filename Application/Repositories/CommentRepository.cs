using MojiiBackend.Domain.Entities;
using MojiiBackend.Infrastructure.Database;

namespace MojiiBackend.Application.Repositories;

public class CommentRepository : BaseCrudRepository<Comment>
{
    public CommentRepository(AppDbContext context) : base(context) {}
    
}