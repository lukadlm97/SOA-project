using AutoMapper;
using Grpc.Core;
using Master.SOA.AuthGrpcApi.Services.Contracts;
using Master.SOA.GrpcProtoLibrary.Protos.Auth;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Master.SOA.AuthGrpcApi.GrpcApi
{
    public class AuthGrpcService : Auth.AuthBase
    {
        private readonly IAuthService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthGrpcService> _logger;

        public AuthGrpcService(IMapper mapper, IAuthService service,ILogger<AuthGrpcService> loggger)
        => (_mapper, _service, _logger) = (mapper, service, loggger);
        public override Task<ChangeRoleReply> UpdateUserRole(UpdateRoleRequest request, ServerCallContext context)
        {
            return base.UpdateUserRole(request, context);
        }
        public override Task<LogInReply> LogIn(LogInRequest request, ServerCallContext context)
        {
            return base.LogIn(request, context);
        }

        public override Task<RegisterReply> RegisterUser(RegisterRequest request, ServerCallContext context)
        {
            return base.RegisterUser(request, context);
        }
    }
}