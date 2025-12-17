using Microsoft.EntityFrameworkCore;
using MojiiBackend.Domain.Entities;
using MojiiBackend.Infrastructure.Database;

namespace MojiiBackend.Application.Repositories;

public class PostRepository : BaseCrudRepository<Post>
{
    public PostRepository(AppDbContext context) : base(context) {}

    public async Task<List<Post>> GetAllPostsByIsThread(bool isThread)
    {
        return await _dbSet.AsNoTracking()
            .Where(p => p.IsThread == isThread)
            .ToListAsync();
    }
}