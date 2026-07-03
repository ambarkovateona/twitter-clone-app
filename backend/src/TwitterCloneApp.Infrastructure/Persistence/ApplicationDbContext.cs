using Microsoft.EntityFrameworkCore;   // ja nosi samata db context klasa, modelbuilder
using TwitterCloneApp.Application.Common.Interfaces;   //IApplicationDbContextInterface
using TwitterCloneApp.Domain.Entities;   //Post i User

namespace TwitterCloneApp.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
//preku db context dobiva konekcija kon bazata, change tracking..
//iapplicationdbcontext - users, posts, savechangesasync
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Post> Posts => Set<Post>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}