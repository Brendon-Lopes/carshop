using Carshop.API.Middleware;
using Carshop.Application;
using Carshop.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddScoped<ExceptionHandlerMiddleware>();

    // dependency injection
    builder.Services
        .AddInfrastructure(builder.Configuration)
        .AddApplication();

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
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
