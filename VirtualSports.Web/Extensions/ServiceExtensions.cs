using System;
using System.IO;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using VirtualSports.BLL.Mappings;
using VirtualSports.BLL.Services;
using VirtualSports.BLL.Services.AdminServices;
using VirtualSports.BLL.Services.AdminServices.Impl;
using VirtualSports.BLL.Services.DatabaseServices;
using VirtualSports.BLL.Services.DatabaseServices.Impl;
using VirtualSports.Web.Authentication;
using VirtualSports.Web.Options;

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

            services.AddScoped<IDatabaseRootService, DatabaseRootService>();
            services.AddScoped<IDatabaseAuthService, DatabaseAuthService>();

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

            services.AddCors();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = JwtOptions.RequireHttps;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = JwtOptions.Issuer,

                        ValidateAudience = true,
                        ValidAudience = JwtOptions.Audience,

                        ValidateLifetime = false,

                        IssuerSigningKey = JwtOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                    };
                });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "VirtualSports", Version = "v1" });
                var filePath = Path.Combine(AppContext.BaseDirectory, "VirtualSports.Web.xml");
                if (File.Exists(filePath))
                {
                    options.IncludeXmlComments(filePath);
                }
                options.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = "Bearer",
                                    Type = ReferenceType.SecurityScheme
                                },
                            },
                            new string[0]
                        }
                    });

                options.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.ApiKey,
                        In = ParameterLocation.Header,
                        Scheme = "Bearer",
                        Name = "Authorization",
                        Description = "JWT token",
                        BearerFormat = "JWT"
                    });
            });

            services
                .AddAuthentication("JwtAuthentication")
                .AddScheme<JwtBearerOptions, AuthenticationHandler>(
                    "JwtAuthentication", null);

            return services;
        }
    }
}
