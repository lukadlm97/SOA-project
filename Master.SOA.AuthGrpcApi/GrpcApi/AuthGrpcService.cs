using AutoMapper;
using Grpc.Core;
using Master.SOA.AuthGrpcApi.Models.Domain;
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

        public AuthGrpcService(IMapper mapper, IAuthService service, ILogger<AuthGrpcService> loggger)
        => (_mapper, _service, _logger) = (mapper, service, loggger);

        public async override Task<ChangeRoleReply> UpdateUserRole(UpdateRoleRequest request, ServerCallContext context)
        {
            var isChanged = await _service.ChangeRole(request.AdminUsername, request.Username, request.Role);

            if (isChanged)
            {
                return new ChangeRoleReply
                {
                    Code = GrpcProtoLibrary.Protos.Auth.StatusCode.Success,
                    Message = "Role of user changed successfully"
                };
            }

            return new ChangeRoleReply
            {
                Code = GrpcProtoLibrary.Protos.Auth.StatusCode.Error,
                Message = $"Role of user cann't be changed"
            };
        }

        public async override Task<LogInReply> LogIn(LogInRequest request, ServerCallContext context)
        {
            var user = _mapper.Map<User>(request);

            var token = await _service.LogIn(user);

            if (token == null)
            {
                return new LogInReply
                {
                    Code = GrpcProtoLibrary.Protos.Auth.StatusCode.Error,
                    GuidId = null
                };
            }

            //TODO say ticker that is login successfully

            return new LogInReply
            {
                Code = GrpcProtoLibrary.Protos.Auth.StatusCode.Success,
                GuidId = token
            };
        }

        public async override Task<RegisterReply> RegisterUser(RegisterRequest request, ServerCallContext context)
        {
            var isCreated = await _service.Register(_mapper.Map<User>(request));

            return isCreated ?
             new RegisterReply
             {
                 Code = GrpcProtoLibrary.Protos.Auth.StatusCode.Success,
                 Message = "user successfully created!!!"
             }
             :
             new RegisterReply
             {
                 Code = GrpcProtoLibrary.Protos.Auth.StatusCode.Error,
                 Message = $"user cann't be created!!!"
             };
        }
    }
}