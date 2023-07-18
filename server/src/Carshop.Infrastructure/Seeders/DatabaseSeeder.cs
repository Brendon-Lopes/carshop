using Carshop.Domain.Models;
using Carshop.Infrastructure.Authentication;
using Carshop.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Carshop.Infrastructure.Seeders;

public class DatabaseSeeder : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public DatabaseSeeder(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await SeedData(dbContext);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private static async Task SeedData(AppDbContext context)
    {
        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();

        // Seeders
        await CreateAdminUser(context);
        await CreateBrandsAndCars(context);

        await context.SaveChangesAsync();
    }

    private static async Task CreateAdminUser(AppDbContext context)
    {
        await context.Users.AddAsync(new User
        {
            FirstName = "Admin",
            LastName = "Admin",
            Email = "admin@mail.com",
            Password = new PasswordHandler().HashPassword("123456"),
            Role = "Admin"
        });
    }

    private static async Task CreateBrandsAndCars(AppDbContext context)
    {
        var fiatBrand = new Brand { Name = "Fiat" };
        var chevroletBrand = new Brand { Name = "Chevrolet" };
        var ferrariBrand = new Brand { Name = "Ferrari" };
        var lamborghiniBrand = new Brand { Name = "Lamborghini" };

        await context.Brands.AddRangeAsync(
            fiatBrand,
            chevroletBrand,
            ferrariBrand,
            lamborghiniBrand);

        await context.Cars.AddRangeAsync(new[]{
            new Car { BrandId = fiatBrand.Id, Name = "Fiat Uno 2010", Model = "Uno", Year = 2010, Price = 25000, ImageUrl = "https://img1.icarros.com/dbimg/galeriaimgmodelo/2/639_1" },
            new Car { BrandId = fiatBrand.Id, Name = "Fiat Palio 2015", Model = "Palio", Year = 2015, Price = 30000, ImageUrl = "https://img1.icarros.com/dbimg/galeriaimgmodelo/2/19150_1" },
            new Car { BrandId = fiatBrand.Id, Name = "Fiat Siena 2018", Model = "Siena", Year = 2018, Price = 35000, ImageUrl = "https://4.bp.blogspot.com/-bRdbGPpmsvI/Wo8N6odK8cI/AAAAAAABCO0/xdbtnUX4Gwk5XOf8USDIZp0BGIog4jimQCLcBGAs/s1600/Fiat-Grand-Siena-2018-1%2B%25281%2529.jpg" },
            new Car { BrandId = fiatBrand.Id, Name = "Fiat Argo 2020", Model = "Argo", Year = 2020, Price = 40000, ImageUrl = "https://1.bp.blogspot.com/-pd8BgM9xB9s/XMO9bOXqNuI/AAAAAAACaxs/a2mvcbVNu1kUFcwGixxpGYqkyUDlXq8MQCLcBGAs/s1600/Argo2020.png" },
            new Car { BrandId = fiatBrand.Id, Name = "Fiat Mobi 2020", Model = "Mobi", Year = 2020, Price = 20000, ImageUrl = "https://www.autossegredos.com.br/wp-content/webp-express/webp-images/uploads/2020/04/fiat-mobi-easy.jpg.webp" },
            new Car { BrandId = fiatBrand.Id, Name = "Fiat Toro 2020", Model = "Toro", Year = 2020, Price = 70000, ImageUrl = "https://www.automaxfiat.com.br/wp-content/uploads/2019/12/pic-thumb-postagens-2-960x480.jpg" },
            new Car { BrandId = chevroletBrand.Id, Name = "Chevrolet Celta 2010", Model = "Celta", Year = 2010, Price = 18000, ImageUrl = "https://www.chavesnamao.com.br/imn/0640X0480/N/veiculos/186889/3439761/chevrolet-celta-em-sao-joao-de-meriti-rj-63a3ebdf.jpg" },
            new Car { BrandId = chevroletBrand.Id, Name = "Chevrolet Onix 2015", Model = "Onix", Year = 2015, Price = 35000, ImageUrl = "https://cdn.motor1.com/images/mgl/1o8Yw/s1/chevrolet-onix-ganha-novidades-na-linha-2015-preco-parte-de-r-33890.webp" },
            new Car { BrandId = chevroletBrand.Id, Name = "Chevrolet Prisma 2018", Model = "Prisma", Year = 2018, Price = 40000, ImageUrl = "https://www.autossegredos.com.br/wp-content/webp-express/webp-images/uploads/2016/12/chevrolet-prisma_1.jpg.webp" },
            new Car { BrandId = chevroletBrand.Id, Name = "Chevrolet Cruze 2020", Model = "Cruze", Year = 2020, Price = 60000, ImageUrl = "https://cdn.motor1.com/images/mgl/yKN4l/s1/chevrolet-cruze-lt-2020.jpg" },
            new Car { BrandId = chevroletBrand.Id, Name = "Chevrolet Tracker 2020", Model = "Tracker", Year = 2020, Price = 75000, ImageUrl = "https://s2.glbimg.com/qRoFPDtTNAJ9yS28b2YdnthNXpg=/0x0:620x413/984x0/smart/filters:strip_icc()/i.s3.glbimg.com/v1/AUTH_cf9d035bf26b4646b105bd958f32089d/internal_photos/bs/2020/m/l/ecjjRJQFqdA3FmfEhTew/2019-06-06-tracker-12.jpg" },
            new Car { BrandId = chevroletBrand.Id, Name = "Chevrolet S10 2020", Model = "S10", Year = 2020, Price = 120000, ImageUrl = "https://s2.glbimg.com/xcFPXYMeyfOBUHMf_xD_zgs8cT0=/0x0:620x413/984x0/smart/filters:strip_icc()/i.s3.glbimg.com/v1/AUTH_cf9d035bf26b4646b105bd958f32089d/internal_photos/bs/2020/B/L/mwVqOWTHOnfZ7ZTf1twA/2020-07-28-s10frente-eziu9qs.jpg" },
            new Car { BrandId = ferrariBrand.Id, Name = "Ferrari F8 Tributo 2021", Model = "F8 Tributo", Year = 2021, Price = 4000000, ImageUrl = "https://s2.glbimg.com/Z22mII2izUsVUjn_RhBvzrhuvc8=/0x0:620x413/984x0/smart/filters:strip_icc()/i.s3.glbimg.com/v1/AUTH_cf9d035bf26b4646b105bd958f32089d/internal_photos/bs/2020/A/m/Ibf2ubSIW7appLRiUMCg/2020-05-30-ferrari-f8-tributo-3.jpg" },
            new Car { BrandId = ferrariBrand.Id, Name = "Ferrari SF90 Stradale 2023", Model = "SF90 Stradale", Year = 2023, Price = 7000000, ImageUrl = "https://1.bp.blogspot.com/-tnfm9UWL2ys/YPXeojcP3LI/AAAAAAAAz0I/RKBhb4_EIb0czzOIO-SlgG1sfxtemctEgCLcBGAsYHQ/s2048/Ferrari-SF80-Stradale-Novitec%2B%25281%2529.jpg" },
            new Car { BrandId = lamborghiniBrand.Id, Name = "Lamborghini Huracán 2021", Model = "Huracán", Year = 2021, Price = 3500000, ImageUrl = "https://s2.glbimg.com/ok_hAXxmpfPIjNfY8KljUAUwstQ=/0x0:620x413/984x0/smart/filters:strip_icc()/i.s3.glbimg.com/v1/AUTH_cf9d035bf26b4646b105bd958f32089d/internal_photos/bs/2020/x/f/QFDrleRJCNQwmhk0GCHg/2019-01-08-lamborghini-huracan-evo-2019-1600-01.jpg" },
            new Car { BrandId = lamborghiniBrand.Id, Name = "Lamborghini Aventador 2022", Model = "Aventador", Year = 2022, Price = 4500000, ImageUrl = "http://www.czechlamborghini.cz/wp-content/uploads/588405-800x445.jpg" },
        });
    }
}