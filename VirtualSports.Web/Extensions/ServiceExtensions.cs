using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using VirtualSports.Web.Authentication;
using VirtualSports.Web.Services.DatabaseServices;

namespace VirtualSports.Web.Extensions
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// Registers authentication services.
        /// </summary>
        /// <typeparam name="TSessionData">Session data type.</typeparam>
        /// <param name="services">Service collection.</param>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddServicesInMemory(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services
               .AddAuthentication("JwtAuthentication")
               .AddScheme<JwtBearerOptions, AuthenticationHandler>(
                   "JwtAuthentication", null);

            //Add database and migration services.
            services.AddDbContext<DatabaseManagerContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DatabaseManagerContext")), ServiceLifetime.Transient);

            services.AddHostedService<MigrationsService>();

            services.AddScoped<IDatabaseRootService, DatabaseRootService>();
            services.AddScoped<IDatabaseAuthService, DatabaseAuthService>();
            services.AddScoped<IDatabaseAdminService, DatabaseAdminService>();

            //Add storage in memory.
            services.AddScoped<ISessionStorage, SessionStorageInMemory>();
            services.AddScoped<IDatabaseUserService, DatabaseUserService>();
            services.AddScoped<IDiceService, DiceService>();

            //Add admin services.
            services.AddScoped<IAdminAddService, AdminAddService>();
            services.AddScoped<IAdminUpdateService, AdminUpdateService>();
            services.AddScoped<IAdminDeleteService, AdminDeleteService>();

            //Add mapper
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            services.AddSingleton(mappingConfig.CreateMapper());
        }
    }
}
