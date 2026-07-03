using Microsoft.EntityFrameworkCore;
using TwitterCloneApp.Domain.Entities;

namespace TwitterCloneApp.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Post> Posts { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}