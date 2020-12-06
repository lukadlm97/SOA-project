using Grpc.Core;
using Master.SOA.GrpcProtoLibrary.Protos.Health;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Master.SOA.TickGrpcApi.Helper
{
    public class HealthServiceImplementation : Health.HealthBase
    {
        private Dictionary<string, HealthCheckResponse.Types.ServingStatus> pairs =
            new Dictionary<string, HealthCheckResponse.Types.ServingStatus>();

        public override async Task<HealthCheckResponse> Check(HealthCheckRequest request, ServerCallContext context)
        {
            HealthCheckResponse.Types.ServingStatus status;

            pairs.TryGetValue(request.Service, out status);

            return new HealthCheckResponse { Status = status };
        }

        public void SetStatus(string service, HealthCheckResponse.Types.ServingStatus status)
        {
            pairs.Add(service, status);
        }

        public void ClearStatus(string service)
        {
            pairs.Remove(service);
        }

        public void ClearAll()
        {
            pairs = new Dictionary<string, HealthCheckResponse.Types.ServingStatus>();
        }
    }
}