using Domain.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace AesEncryptor;

public static class Startup
{
    public static void AddAesEncryptor(this IServiceCollection services, string? key)
    {
        services.AddScoped<IEncryptor>(p => new AesEncryptorService(key));
    }
}
