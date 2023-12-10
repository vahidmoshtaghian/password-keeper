using Microsoft.OpenApi.Models;

namespace ApiGateway.Extensions;

public static class SwaggerExtensions
{
    public static void AddSwaggerLibrary(this IServiceCollection services)
    {
        services.AddSwaggerGen(opt =>
        {
            foreach (var definition in SwaggerDefinition.GetAll())
            {
                opt.SwaggerDoc(definition, new OpenApiInfo { Title = definition, Version = "v1" });
            }
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });
            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }

    public static void UseSwaggerLibrary(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            foreach (var definition in SwaggerDefinition.GetAll())
            {
                c.SwaggerEndpoint($"/swagger/{definition}/swagger.json", definition);
            }
        });
    }
}

public class SwaggerDefinition
{
    public const string UserArea = "User Area";

    public static string[] GetAll()
    {
        var def = new SwaggerDefinition();
        return def.GetType().GetFields().Select(p => p.GetValue(def).ToString()).ToArray();
    }
}