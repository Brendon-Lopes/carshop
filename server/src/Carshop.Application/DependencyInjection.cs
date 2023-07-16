using Carshop.Application.Interfaces.Brand;
using Carshop.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Carshop.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IBrandService, BrandService>();

        return services;
    }
}