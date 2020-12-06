using Master.SOA.TickGrpcApi.Helper;
using Master.SOA.TickGrpcApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Master.SOA.TickGrpcApi
{
    public class Startup
    {
        private readonly JwtSecurityTokenHandler JwtTokenHandler = new JwtSecurityTokenHandler();
        private static readonly SymmetricSecurityKey SecurityKey = new SymmetricSecurityKey(Guid.NewGuid().ToByteArray());
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
            services.AddMemoryCache();



            services.AddAuthorization(options =>
            {
                options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireClaim(ClaimTypes.Role);
                });
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateAudience = false,
                            ValidateIssuer = false,
                            ValidateActor = false,
                            ValidateLifetime = true,
                            IssuerSigningKey = SecurityKey
                        };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<GreeterService>();
                endpoints.MapGrpcService<TickerService>();
                endpoints.MapGrpcService<InterProccessService>();
                endpoints.MapGrpcService<HealthServiceImplementation>();

                endpoints.MapGet("/generateJwtToken", context =>
                {
                    return context.Response.WriteAsync(GenerateJwtToken(context.Request.Query["role"]));
                });
            });
        }
        private  string GenerateJwtToken(string role)
        {
            if (string.IsNullOrEmpty(role))
            {
                LogTokenGenerater(role, true);
                throw new Exception("Name is not specified.");
            }
            LogTokenGenerater(role, false);

            var claims = new[] { new Claim(ClaimTypes.Role, role) };
            var credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken("ExampleServer", "ExampleClients", claims, expires: DateTime.Now.AddSeconds(5000), signingCredentials: credentials);
            return JwtTokenHandler.WriteToken(token);
        }

        private void LogTokenGenerater(string name, bool errorOccured)
        {
            var consoleClassic = Console.ForegroundColor;
            if (errorOccured)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"error: ");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"info: ");
            }
            Console.ForegroundColor = consoleClassic;
            Console.Write($"Authentification of {name} has been ");
            Console.WriteLine(errorOccured ? "unsuccessfully" : "successfully");
        }
    }
}