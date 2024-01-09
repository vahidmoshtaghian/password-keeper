using SqlServerOrm;

namespace BackendTests.Base;

public class ApplicationInMemoryDbContext : ApplicationDbContext
{
    public ApplicationInMemoryDbContext() : base(CreateOptions())
    {
    }

    private static DbContextOptions<ApplicationDbContext> CreateOptions()
    {
        DbContextOptionsBuilder<ApplicationDbContext> builder = new();
        builder.UseInMemoryDatabase($"TestBackendDb_{Guid.NewGuid()}");

        return builder.Options;
    }
}