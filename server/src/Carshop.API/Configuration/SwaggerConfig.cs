using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace Carshop.API.Configuration;

public static class SwaggerConfig
{
    public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Carshop API", Version = "v1" });

            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Bearer Token",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securityScheme, new List<string>() }
            });
        });

        return services;
    }
}