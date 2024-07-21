using Rentt.Data;
using Rentt.Repositories;
using Rentt.Services;

namespace Rentt.Infrastructure
{
    public static class DependencyInjectionConfig
    {
        public static void ConfigureDependencies(this IServiceCollection services)
        {
            services.AddSingleton<MongoDbService>();

            AddRepositories(services);
            AddServices(services);
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<MotorcycleRepository>();
            services.AddScoped<RentalPlanRepository>();
            services.AddScoped<RentRepository>();
            services.AddScoped<UserRepository>();
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<FileService>();
            services.AddScoped<MotorcycleService>();
            services.AddScoped<RentService>();
            services.AddScoped<UserService>();
        }
    }
}