using Grpc.Core;
using Master.SOA.GrpcProtoLibrary.Protos.Greeter;
using System.Threading.Tasks;

namespace Master.SOA.TickGrpcApi.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        public override async Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return new HelloReply
            {
                Message = "Hello " + request.Name
            };
        }
    }
}