using Rentt.Data;
using Rentt.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<MongoDbService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var scope = app.Services.CreateScope();

var rentPlanRepository = scope.ServiceProvider.GetRequiredService<RentalPlanRepository>();
rentPlanRepository.SeedData();

app.Run();