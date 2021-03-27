using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtualSports.BLL.Services;
using VirtualSports.DAL.Contexts;
using VirtualSports.DAL.Repositories;
using VirtualSports.DAL.Repositories.Interfaces;

namespace VirtualSports.BLL.Extensions
{
    public static class DatabaseServiceExtensions
    {
        public static IServiceCollection AddDatabaseServicesInMemory(this IServiceCollection services, IConfiguration configuration)
        {
            //Add database and migration services.
            services.AddDbContext<DatabaseManagerContext>(
                options => options.UseNpgsql(configuration.GetConnectionString("DatabaseManagerContext"),
                options => options.MigrationsAssembly("VirtualSports.Web")));

            services.AddHostedService<MigrationsService>();
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            return services;
        }
    }
}
