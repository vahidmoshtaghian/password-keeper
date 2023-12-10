using Domain.Contracts;
using Domain.Exceptions.UserExceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SqlServerOrm;

public static class Startup
{
    public static void AddSqlServerOrm(this IServiceCollection services, string? connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new EmptyConnectionStringException();

        services.AddDbContext<IContext, ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
    }

    public static void UseSqlServerOrm(this IApplicationBuilder app)
    {
        using var provider = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = provider.ServiceProvider.GetRequiredService<IContext>();
        context.Database.Migrate();
    }
}