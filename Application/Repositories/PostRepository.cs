using Microsoft.EntityFrameworkCore;
using MojiiBackend.Domain.Entities;
using MojiiBackend.Infrastructure.Database;

namespace MojiiBackend.Application.Repositories;

public class PostRepository(AppDbContext context)
    : BaseCrudRepository<Post>(context)
{
    private IQueryable<Post> BuildFeedQuery()
    {
        return _dbSet
            .AsNoTracking()
            .Include(p => p.User)
            .ThenInclude(u => u.Organization)
            .Include(p => p.User)
            .ThenInclude(u => u.Filiere)
            .Include(p => p.HavingLikedUsers)
            .ThenInclude(u => u.Organization)
            .Include(p => p.HavingLikedUsers)
            .ThenInclude(u => u.Filiere)
            .Include(p => p.Comments)
            .ThenInclude(c => c.User)
            .ThenInclude(u => u.Organization)
            .Include(p => p.Comments)
            .ThenInclude(c => c.User)
            .ThenInclude(u => u.Filiere);
    }

    public override async Task<List<Post>> GetAll()
    {
        return await BuildFeedQuery()
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Post>> GetMostRecentWithSkip(int skip)
    {
        return await BuildFeedQuery()
            .OrderByDescending(p => p.CreatedAt)
            .Skip(skip)
            .ToListAsync();
    }

    public override async Task<Post?> GetById(int id)
    {
        return await BuildFeedQuery()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Post>> GetPostsByUserId(int userId)
    {
        return await BuildFeedQuery()
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<Post?> GetByIdForUpdate(int id)
    {
        return await _dbSet.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddLike(int postId, int userId)
    {
        var post = await _dbSet
            .Include(p => p.HavingLikedUsers)
            .FirstOrDefaultAsync(p => p.Id == postId);

        if (post == null)
            throw new Exception("Post not found");

        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            throw new Exception("User not found");

        if (!post.HavingLikedUsers.Any(u => u.Id == userId))
        {
            post.HavingLikedUsers.Add(user);
            post.NbOfLikes++;
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveLike(int postId, int userId)
    {
        var post = await _dbSet
            .Include(p => p.HavingLikedUsers)
            .FirstOrDefaultAsync(p => p.Id == postId);

        if (post == null)
            throw new Exception("Post not found");

        var user = post.HavingLikedUsers.FirstOrDefault(u => u.Id == userId);
        if (user != null)
        {
            post.HavingLikedUsers.Remove(user);
            post.NbOfLikes--;
            await _context.SaveChangesAsync();
        }
    }
}
