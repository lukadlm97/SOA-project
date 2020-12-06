using Master.SOA.GrpcProtoLibrary.Protos.Health;
using Master.SOA.TickGrpcApi.Helper;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Master.SOA.TickGrpcApi.Services
{
    public class StatusService : BackgroundService
    {
        private readonly HealthServiceImplementation _healthService;
        private readonly HealthCheckService _healthCheckService;

        public StatusService(HealthServiceImplementation healthServiceImpl, HealthCheckService healthCheckService)
            => (_healthService, _healthCheckService) = (healthServiceImpl, healthCheckService);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var health = await _healthCheckService.CheckHealthAsync(stoppingToken);

                _healthService.SetStatus("Ticker",
                    health.Status == HealthStatus.Healthy
                    ? HealthCheckResponse.Types.ServingStatus.Serving
                    : HealthCheckResponse.Types.ServingStatus.NotServing);

                _healthService.SetStatus("Greeter",
                    health.Status == HealthStatus.Healthy
                    ? HealthCheckResponse.Types.ServingStatus.Serving
                    : HealthCheckResponse.Types.ServingStatus.NotServing);

                _healthService.SetStatus("InterProccessService",
                    health.Status == HealthStatus.Healthy
                    ? HealthCheckResponse.Types.ServingStatus.Serving
                    : HealthCheckResponse.Types.ServingStatus.NotServing);

                _healthService.SetStatus(string.Empty,
                   health.Status == HealthStatus.Healthy
                       ? HealthCheckResponse.Types.ServingStatus.Serving
                       : HealthCheckResponse.Types.ServingStatus.NotServing);

                await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
            }
        }
    }
}