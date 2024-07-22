using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Rentt.Entities;
using Rentt.Infrastructure;
using Rentt.Infrastructure.Authentication;
using Rentt.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(serviceProvider =>
{
    var client = new MongoClient(builder.Configuration.GetConnectionString("DbConnection"));
    return client.GetDatabase("rentt");
});

builder.Services.AddSingleton<IUserStore<User>, UserStore>();
builder.Services.AddSingleton<IRoleStore<IdentityRole>, RoleStore>();

builder.Services.AddIdentity<User, IdentityRole>()
    .AddDefaultTokenProviders();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

var scope = app.Services.CreateScope();

var rentPlanRepository = scope.ServiceProvider.GetRequiredService<RentalPlanRepository>();
rentPlanRepository.SeedData();

app.Run();