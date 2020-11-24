using Master.SOA.Domain.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Master.SOA.TickGrpcApi
{
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
                    webBuilder.ConfigureServices((context, services) =>
                    {
                        services.AddOptions();
                        services.Configure<ConfigSettings>(context.Configuration.GetSection("Database"));
                    });
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://localhost:45678", "https://localhost:45679");
                });
    }
}