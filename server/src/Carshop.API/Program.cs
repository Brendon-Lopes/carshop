using Carshop.API.Configuration;
using Carshop.API.Middleware;
using Carshop.Application;
using Carshop.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddScoped<ExceptionHandlerMiddleware>();

    // Dependency injection
    builder.Services
        .AddInfrastructure(builder.Configuration)
        .AddApplication();

    // Authentication / Authorization
    builder.Services
        .ConfigureAuthentication(builder.Configuration)
        .ConfigureAuthorization();

    builder.Services.ConfigureCors();

    builder.Services.AddAutoMapper(typeof(Program).Assembly);
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseMiddleware<ExceptionHandlerMiddleware>();
    app.UseHttpsRedirection();

    app.UseCors("CorsPolicy");

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.Run();
}
