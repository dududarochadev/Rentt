using Microsoft.OpenApi.Models;
using Rentt.Infrastructure;
using Rentt.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rentt", Version = "v1" });
});

builder.Services.ConfigureDependencies();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rentt v1");
    });
}

app.UseHttpsRedirection();

app.MapControllers();

var scope = app.Services.CreateScope();

var rentPlanRepository = scope.ServiceProvider.GetRequiredService<RentalPlanRepository>();
rentPlanRepository.SeedData();

app.Run();