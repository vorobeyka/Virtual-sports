using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace VirtualSports.Web
{
#pragma warning disable 1591
    public class Program

    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                }).ConfigureLogging(logginBuilder =>
                {
                    logginBuilder.ClearProviders();
                    logginBuilder.SetMinimumLevel(LogLevel.Trace);
                    logginBuilder.AddSerilog(new LoggerConfiguration()
                        .WriteTo.Console()
                        .WriteTo.File("Vi.log")
                        .CreateLogger());
                });
    }
#pragma warning restore 1591
}
