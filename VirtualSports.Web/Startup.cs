using System;
using System.IO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using VirtualSports.Web.Contexts;
using VirtualSports.Web.Services;
using VirtualSports.Web.Services.DatabaseServices;
using VirtualSports.Web.Options;
using VirtualSports.Web.Authentication;

namespace VirtualSports.Web
{

#pragma warning disable 1591
    public class Startup

    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
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

                        ValidateLifetime = true,

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

            //Add database and migration services.
            services.AddDbContext<DatabaseManagerContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DatabaseManagerContext")), ServiceLifetime.Transient);

            services.AddHostedService<MigrationsService>();

            services.AddScoped<IDatabaseRootService, DatabaseRootService>();
            services.AddScoped<IDatabaseAuthService, DatabaseAuthService>();

            //Add storage in memory.
            services.AddScoped<ISessionStorage, SessionStorageInMemory>();
            services.AddScoped<IDatabaseUserService, DatabaseUserService>();
            services.AddScoped<IDiceService, DiceService>();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Virtual Sports API V1");
                });
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
#pragma warning restore 1591
}
