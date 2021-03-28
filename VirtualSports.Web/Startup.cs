using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtualSports.Web.Extensions;
using VirtualSports.BLL.Extensions;
using VirtualSports.DAL.Contexts;
using VirtualSports.BLL.Services;
using Microsoft.EntityFrameworkCore;
using VirtualSports.Web.Middleware;

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
            services.AddServicesInMemory();
            
            services.AddDatabaseServicesInMemory(Configuration);
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Virtual Sports API V1");
                });
            //}

            app.UseCors(builder => builder
                .WithOrigins(
                    "https://virtual-sports.github.io",
                    "http://localhost:3000",
                    "http://localhost:5000",
                    "https://virtual-sports-yi3j9.ondigitalocean.app")
                .AllowAnyHeader()
                .AllowAnyMethod());

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<LoggingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
#pragma warning restore 1591
}
