using System;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtualSports.BLL.Mappings;
using VirtualSports.BLL.Services;
using VirtualSports.BLL.Services.AdminServices;
using VirtualSports.BLL.Services.AdminServices.Impl;
using VirtualSports.BLL.Services.DatabaseServices;
using VirtualSports.BLL.Services.DatabaseServices.Impl;
using VirtualSports.DAL.Contexts;
using VirtualSports.Web.Authentication;
using VirtualSports.Web.Services;

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
        public static IServiceCollection AddServicesInMemory(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services
               .AddAuthentication("JwtAuthentication")
               .AddScheme<JwtBearerOptions, AuthenticationHandler>(
                   "JwtAuthentication", null);

            //Add database and migration services.
            services.AddDbContext<DatabaseManagerContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DatabaseManagerContext")), ServiceLifetime.Transient);

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

            return services;
        }
    }
}
