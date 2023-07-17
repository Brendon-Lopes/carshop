using Carshop.Application.Interfaces.Authentication;
using Carshop.Application.Interfaces.Brand;
using Carshop.Application.Interfaces.User;
using Carshop.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Carshop.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IBrandService, BrandService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        return services;
    }
}