using Carshop.Application.Interfaces.Authentication;
using Carshop.Domain.Interfaces;
using Carshop.Infrastructure.Authentication;
using Carshop.Infrastructure.Context;
using Carshop.Infrastructure.Repositories;
using Carshop.Infrastructure.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Carshop.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment env)
    {
        if (env.IsEnvironment("Test"))
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("TestDb"));
        }
        else
        {
            services.AddDbContext<AppDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            );
        }

        services.AddScoped<IBrandRepository, BrandRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICarRepository, CarRepository>();

        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IPasswordHandler, PasswordHandler>();

        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

        if (!env.IsEnvironment("Test"))
        {
            services.AddHostedService<DatabaseSeeder>();
        }

        return services;
    }
}