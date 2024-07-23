using Rentt.Infrastructure;
using Rentt.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMongoDb(builder.Configuration);
builder.Services.AddIdentity();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddRabbitMQ(builder.Configuration);
builder.Services.AddSwagger();
builder.Services.AddDependencyInjection();

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

var rentPlanRepository = scope.ServiceProvider.GetRequiredService<IRentalPlanRepository>();
rentPlanRepository.SeedData();

app.Run();