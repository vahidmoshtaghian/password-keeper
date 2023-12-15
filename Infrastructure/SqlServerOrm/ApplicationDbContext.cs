using Domain.Contracts;
using Domain.Entities.Actor;
using Domain.Entities.Guard;
using Microsoft.EntityFrameworkCore;

namespace SqlServerOrm;

internal class ApplicationDbContext : DbContext, IContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    #region Actor

    public DbSet<Friend> Friends { get; set; }
    public DbSet<Person> People { get; set; }
    public DbSet<User> Users { get; set; }

    #endregion

    #region Guard

    public DbSet<Credential> Credentials { get; set; }
    public DbSet<Membership> Memberships { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Password> Passwords { get; set; }
    public DbSet<UserAccess> UserAccesses { get; set; }

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // TODO Deleted items query filter

        modelBuilder.Entity<Friend>()
            .HasOne(p => p.Owner)
            .WithMany(p => p.Friends)
            .HasForeignKey(p => p.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // TODO Soft delete items

        return base.SaveChangesAsync(cancellationToken);
    }
}
