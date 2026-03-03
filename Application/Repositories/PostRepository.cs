using Microsoft.EntityFrameworkCore;
using MojiiBackend.Domain.Entities;
using MojiiBackend.Infrastructure.Database;

namespace MojiiBackend.Application.Repositories;

public class PostRepository : BaseCrudRepository<Post>
{
    public PostRepository(AppDbContext context) : base(context) {}
}