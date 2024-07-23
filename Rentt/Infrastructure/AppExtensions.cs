using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Rentt.Bus;
using Rentt.Data;
using Rentt.Entities;
using Rentt.Infrastructure.Authentication;
using Rentt.Repositories;
using Rentt.Services;

namespace Rentt.Infrastructure
{
    public static class AppExtensions
    {
        public static void AddDependencyInjection(this IServiceCollection services)
        {
            AddRepositories(services);
            AddServices(services);
        }
        public static void AddIdentity(this IServiceCollection services)
        {
            services.AddSingleton<IUserStore<User>, UserStore>();
            services.AddSingleton<IRoleStore<IdentityRole>, RoleStore>();

            services.AddIdentity<User, IdentityRole>()
                .AddDefaultTokenProviders();
        }

        public static void AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(serviceProvider =>
            {
                var client = new MongoClient(configuration.GetConnectionString("DbConnection"));
                return client.GetDatabase("rentt");
            });

            services.AddSingleton<MongoDbService>();
        }

        public static void AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            var uri = configuration["RabbitMQHost"] ?? string.Empty;

            services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.AddConsumer<MotorcycleCreatedEventConsumer>();

                busConfigurator.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(new Uri(uri), host =>
                    {
                        host.Username("guest");
                        host.Password("guest");
                    });

                    cfg.ConfigureEndpoints(ctx);
                });
            });
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rentt", Version = "v1" });
            });
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IDeliverymanRepository, DeliverymanRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
            services.AddScoped<IRentalPlanRepository, RentalPlanRepository>();
            services.AddScoped<IRentRepository, RentRepository>();
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<IDeliverymanService, DeliverymanService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IMotorcycleService, MotorcycleService>();
            services.AddScoped<IRentalPlanService, RentalPlanService>();
            services.AddScoped<IRentService, RentService>();
        }
    }
}