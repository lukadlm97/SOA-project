using Grpc.Core;
using Master.SOA.BusinessLogic.Contracts;
using Master.SOA.GrpcProtoLibrary.Protos.Interprocess;
using System.Threading.Tasks;

namespace Master.SOA.TickGrpcApi.Services
{
    public class InterProccessService : InterprocessCommunication.InterprocessCommunicationBase
    {
        private readonly IAuthService _service;

        public InterProccessService(IAuthService service) =>
                (_service) = (service);

        public async override Task<HelloReply> SayGuid(HelloRequest request, ServerCallContext context)
        {
            var isCommited = await _service.SetParams(request.Guid, request.Role);

            return isCommited? 
             new HelloReply 
             { 
                 Status = GrpcProtoLibrary.Protos.Interprocess.StatusCode.Success 
             }
             :
             new HelloReply { 
                 Status = GrpcProtoLibrary.Protos.Interprocess.StatusCode.Error 
             };
        }


    }
}