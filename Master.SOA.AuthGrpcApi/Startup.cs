using AutoMapper;
using Master.SOA.AuthGrpcApi.GrpcApi;
using Master.SOA.AuthGrpcApi.Models.Dbo;
using Master.SOA.AuthGrpcApi.Repositories;
using Master.SOA.AuthGrpcApi.Repositories.Contracts;
using Master.SOA.AuthGrpcApi.Services;
using Master.SOA.AuthGrpcApi.Services.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Master.SOA.AuthGrpcApi
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();

            services.AddSingleton<IAuthRepository<UserDbo>, AuthRepository>();
            services.AddSingleton<IAuthService, AuthService>();

            services.AddOptions();
            services.AddAutoMapper(typeof(AutoMapperProfile));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<AuthGrpcService>();
            });
        }
    }
}