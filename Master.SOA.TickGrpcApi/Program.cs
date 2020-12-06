using AutoMapper;
using Master.SOA.BusinessLogic.Contracts;
using Master.SOA.BusinessLogic.Repositories;
using Master.SOA.BusinessLogic.Services;
using Master.SOA.Domain.Configurations;
using Master.SOA.Domain.DataTransferObjects;
using Master.SOA.Domain.Models;
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
                        services.AddSingleton<IDataService<Tick>, TickService>();
                        services.AddSingleton<IDataRepository<TickDto>, TickRepository>();
                        services.AddSingleton<IInstrumentService, InstrumentService>();
                        services.AddSingleton<IAuthRepository, AuthRepository>();
                        services.AddSingleton<IAuthService, AuthService>();
                        services.AddAutoMapper(typeof(AutoMapperProfile));
                    });
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://localhost:45678", "https://localhost:45679");
                });
    }
}