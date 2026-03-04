using Microsoft.EntityFrameworkCore;
using MojiiBackend.Infrastructure.Database;

namespace MojiiBackend.Application.Repositories;

public class BaseCrudRepository<T> (AppDbContext context) where T : class
{
    protected readonly AppDbContext _context = context;

    /// <summary>
    /// Represents the DbSet instance for the specified entity type <typeparamref name="T"/>.
    /// Used to perform CRUD operations and query the database for entities of type <typeparamref name="T"/>.
    /// Useful to query directly without passing by the context
    /// </summary>
    protected readonly DbSet<T> _dbSet = context.Set<T>();
    
    
    public virtual async Task<T?> GetById(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<List<T>> GetAll()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task Create(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<T> Update(T entity)
    {
        var entry = _context.Entry(entity);
                
        if (entry.State == EntityState.Detached)
        {
            _dbSet.Update(entity);
        }        
        
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> Exists(int id)
    {
        return await _dbSet.FindAsync(id) is not null;
    }

    public async Task Delete(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
    
    public virtual async Task<T> DeleteExistingEntityAndReturn(T entity)
    {
        var deletedEntity = _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
        return deletedEntity.Entity;
    }
}