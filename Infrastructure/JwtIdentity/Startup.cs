using Domain.Base;
using Domain.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JwtIdentity;

public static class Startup
{
    internal static string SecretKey = string.Empty;

    public static void AddJwtIdentity(this IServiceCollection services, string? secretKey)
    {
        SecretKey = secretKey;
        services.AddScoped<IIdentityService, AuthentiaionService>();

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                //ValidIssuer = "https://localhost:5001",
                //ValidAudience = "https://localhost:5001",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };
        });

        services.AddScoped(provider =>
        {
            var accessor = provider.GetRequiredService<IHttpContextAccessor>();
            var user = accessor.HttpContext.User.Claims;

            return new CurrentUser
            {
                Id = Convert.ToInt64(user.FirstOrDefault(p => p.Type == "id")?.Value)
            };
        });
    }

    public static void UseJwtIdentity(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}