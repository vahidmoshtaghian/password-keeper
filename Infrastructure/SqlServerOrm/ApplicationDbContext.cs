using Domain.Contracts;
using Domain.Entities.Actor;
using Microsoft.EntityFrameworkCore;

namespace SqlServerOrm;

internal class ApplicationDbContext : DbContext, IContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<Person> People { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // TODO Deleted items query filter

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // TODO Soft delete items

        return base.SaveChangesAsync(cancellationToken);
    }
}
